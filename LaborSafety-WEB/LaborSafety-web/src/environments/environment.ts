import { apiPaths } from './api.environment';

// CONFIGURACAO DTI
/*
const teste07 = "07";
const useOnlyHttps = false;
const protocol = window.location.href.trim().toLowerCase().indexOf('https') === -1 ? "http://" : "https://";

export const environment = {
  production: false,
  host: `${protocol}${'localhost:51249'}/api/`,
  loginHost: `${protocol}${'localhost:51249'}/`,
  apiPaths,
  onlyHttps: useOnlyHttps
};
*/


//CONFIGURACAO TERNIUM
const teste10 = "10";
const useOnlyHttps = false;
const protocol = window.location.href.trim().toLowerCase().indexOf('https') === -1 ? "http://" : "https://";

export const environment = {
  production: false,
  //host: `${protocol}${window['host']}/aprpt.api/Api/`,
  host: `${protocol}${window['api']}`,
  loginHost: `${protocol}${window['login']}`,
  apiPaths,
  onlyHttps: useOnlyHttps
};
