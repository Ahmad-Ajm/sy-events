import { Environment } from '@abp/ng.core';

const baseUrl = '{{FRONTEND_URL}}';

export const environment = {
  production: false,
  application: {
    baseUrl,
    name: '{{APP_NAME}}',
    logoUrl: '/assets/logo.png',
  },
  oAuthConfig: {
    issuer: '{{AUTH_AUTHORITY}}/',
    redirectUri: baseUrl,
    clientId: '{{CLIENT_ID}}',
    responseType: 'code',
    scope: 'offline_access {{API_SCOPE}}',
    requireHttps: true,
  },
  apis: {
    default: {
      url: '{{BACKEND_URL}}',
      rootNamespace: '{{ROOT_NAMESPACE}}',
    },
  },
} as Environment;
