import { BASE_API_URL } from "./apiConstatns";

const ACCOUNT_API_URL = `account`;

export async function getToken(login, password) {
  const url = ACCOUNT_API_URL + '/token';
  const headers = new Headers();
  headers.set('Authorization', 'Basic ' + btoa(login + ':' + password));

  const requestOptions = {
    method: "POST",
    headers
  };

  const resultFetch = await fetch(url, requestOptions);
  if (resultFetch.ok) {
    const result = await resultFetch.json();
    return result.accessToken;
  }
  else {
    throw new Error('Ошибка ' + resultFetch.status + ': ' + resultFetch.statusText);
  }
}


