const protocol: string = window.location.protocol;
const apiDomain: string = 'localhost:5207';
const baseUrl: string = `${protocol}//${apiDomain}`;

export const environment = {
  production: true,
  apiUrl: `${baseUrl}/api/`,
  baseUrl: `${baseUrl}/`,
  allowedDomains: [apiDomain],
  disallowedRoutes: [`${apiDomain}/api/auth`],
  noImageSrc: `${baseUrl}/assets/img/no-image.jpg`,
};