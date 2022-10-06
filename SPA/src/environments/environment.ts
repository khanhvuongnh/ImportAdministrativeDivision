const port: string = ':5207';
const protocol: string = window.location.protocol;
const hostname: string = window.location.hostname;
const hostport: string = `${hostname}${port}`;
const baseUrl: string = `${protocol}//${hostport}`;

export const environment = {
  production: false,
  baseUrl: baseUrl,
  apiUrl: `${baseUrl}/api`,
  allowedDomains: [hostport],
  disallowedRoutes: [`${hostport}/api/auth`],
};