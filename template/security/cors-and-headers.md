# Security: CORS & Headers (Template)

## CORS
- Origins: {{CORS_ORIGINS}}
- AllowCredentials: {{ALLOW_CREDENTIALS}}
- AllowedHeaders: Authorization, Content-Type, Accept
- AllowedMethods: GET, POST, PUT, DELETE, PATCH, OPTIONS

## Security Headers (Reverse Proxy or Host)
- Strict-Transport-Security: max-age=31536000; includeSubDomains
- X-Content-Type-Options: nosniff
- X-Frame-Options: SAMEORIGIN
- Referrer-Policy: no-referrer-when-downgrade
- Content-Security-Policy: default-src 'self'; img-src 'self' data:; object-src 'none'
