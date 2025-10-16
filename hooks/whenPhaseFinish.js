#!/usr/bin/env node
/*
  Hook: whenPhaseFinish
  Purpose: After finishing a phase, verify running services and update docs/status.
  Usage:
    node hooks/whenPhaseFinish.js --phase phase2 --status completed --notes "Domain layer copied"
*/

const fs = require('fs');
const path = require('path');
const https = require('https');
const http = require('http');

function parseArgs() {
  const args = process.argv.slice(2);
  const options = {};
  for (let i = 0; i < args.length; i++) {
    const arg = args[i];
    if (arg.startsWith('--')) {
      const key = arg.slice(2);
      const value = args[i + 1] && !args[i + 1].startsWith('--') ? args[++i] : 'true';
      options[key] = value;
    }
  }
  return options;
}

function getRoot() {
  // Ensure this script always runs relative to repository root regardless of CWD
  const here = __dirname; // .../CS-SY-Events/hooks
  return path.resolve(here, '..'); // .../CS-SY-Events
}

function httpGet(url) {
  return new Promise((resolve) => {
    const isHttps = url.startsWith('https://');
    const lib = isHttps ? https : http;
    const req = lib.get(
      url,
      isHttps
        ? { rejectUnauthorized: false, timeout: 8000 }
        : { timeout: 8000 },
      (res) => {
        // drain data
        res.on('data', () => {});
        res.on('end', () => resolve({ ok: res.statusCode >= 200 && res.statusCode < 400, status: res.statusCode }));
      }
    );
    req.on('error', (err) => resolve({ ok: false, error: err.message }));
    req.on('timeout', () => {
      req.destroy();
      resolve({ ok: false, error: 'timeout' });
    });
  });
}

function appendStatusLog(rootDir, log) {
  const statusFile = path.join(rootDir, 'STATUS.md');
  const ts = new Date().toISOString();
  const entry = `\n- ${ts} — Phase: ${log.phase} — Status: ${log.status} — API: ${log.api.ok ? 'OK' : 'FAIL'} (${log.api.status || log.api.error}) — UI: ${log.ui.ok ? 'OK' : 'FAIL'} (${log.ui.status || log.ui.error}) — Notes: ${log.notes || ''}`;
  try {
    let content = fs.readFileSync(statusFile, 'utf8');
    if (!content.includes('## 🧪 Automation Log')) {
      content += `\n\n## 🧪 Automation Log\n`;
    }
    content += entry + '\n';
    fs.writeFileSync(statusFile, content, 'utf8');
  } catch (e) {
    // If STATUS.md is missing, write to Logs/ instead
    const logsDir = path.join(rootDir, 'Logs');
    if (!fs.existsSync(logsDir)) fs.mkdirSync(logsDir, { recursive: true });
    fs.appendFileSync(path.join(logsDir, 'phase-updates.log'), entry + '\n', 'utf8');
  }
}

function updatePlanChecklist(rootDir, phase, status) {
  // Light-touch: replace the phase header marker from pending to completed where applicable
  const planFile = path.join(rootDir, 'PLAN.md');
  if (!fs.existsSync(planFile)) return;
  let content = fs.readFileSync(planFile, 'utf8');
  const map = {
    phase1: /### Phase 1:([^\n]*?)\n\*\*الحالة:\*\*[^\n]*/i,
    phase2: /### Phase 2:([^\n]*?)\n\*\*الحالة:\*\*[^\n]*/i,
    phase3: /### Phase 3:([^\n]*?)\n\*\*الحالة:\*\*[^\n]*/i,
    phase4: /### Phase 4:([^\n]*?)\n\*\*الحالة:\*\*[^\n]*/i,
    phase5: /### Phase 5:([^\n]*?)\n\*\*الحالة:\*\*[^\n]*/i,
    phase6: /### Phase 6:([^\n]*?)\n\*\*الحالة:\*\*[^\n]*/i,
    phase7: /### Phase 7:([^\n]*?)\n\*\*الحالة:\*\*[^\n]*/i,
    phase8: /### Phase 8:([^\n]*?)\n\*\*الحالة:\*\*[^\n]*/i,
    phase9: /### Phase 9:([^\n]*?)\n\*\*الحالة:\*\*[^\n]*/i,
    phase10: /### Phase 10:([^\n]*?)\n\*\*الحالة:\*\*[^\n]*/i,
    phase11: /### Phase 11:([^\n]*?)\n\*\*الحالة:\*\*[^\n]*/i,
    phase12: /### Phase 12:([^\n]*?)\n\*\*الحالة:\*\*[^\n]*/i,
  };
  const rx = map[String(phase).toLowerCase()];
  if (rx) {
    content = content.replace(rx, (m) => m.replace(/\*\*الحالة:\*\*[^\n]*/i, `**الحالة:** ${status.toLowerCase() === 'completed' ? '✅ مكتمل' : '⏳ جاري التنفيذ'}`));
    fs.writeFileSync(planFile, content, 'utf8');
  }
}

async function main() {
  const opts = parseArgs();
  const root = getRoot();
  const phase = opts.phase || 'unknown';
  const status = opts.status || 'completed';
  const notes = opts.notes || '';

  // Verify services
  const [api, ui] = await Promise.all([
    httpGet('https://localhost:44388/api/abp/application-configuration'),
    httpGet('http://localhost:4200/'),
  ]);

  // Log to STATUS.md (or Logs/)
  appendStatusLog(root, { phase, status, notes, api, ui });
  // Update PLAN.md header for the phase
  updatePlanChecklist(root, phase, status);

  // Also append a machine-readable log
  const logsDir = path.join(root, 'Logs');
  if (!fs.existsSync(logsDir)) fs.mkdirSync(logsDir, { recursive: true });
  const obj = { ts: new Date().toISOString(), phase, status, notes, api, ui };
  fs.appendFileSync(path.join(logsDir, 'phase-updates.jsonl'), JSON.stringify(obj) + '\n', 'utf8');

  // Console summary
  const summary = `Phase=${phase} Status=${status} API=${api.ok ? 'OK' : 'FAIL'} UI=${ui.ok ? 'OK' : 'FAIL'}`;
  console.log(summary);
  if (!api.ok || !ui.ok) process.exitCode = 2;
}

main().catch((err) => {
  console.error('whenPhaseFinish failed:', err);
  process.exit(1);
});



