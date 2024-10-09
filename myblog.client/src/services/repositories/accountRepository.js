import { BASE_URL } from "../appConstants";
import { ACCOUNT_API_URL, BASE_API_URL, TOKEN_NAME } from "./apiConstatns";

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
    localStorage.clear();
    localStorage.setItem(TOKEN_NAME, result.accessToken);
    return result.accessToken;
  }
  else {
    throw new Error('Ошибка ' + resultFetch.status + ': ' + resultFetch.statusText);
  }
}

export async function getCurrentUser() {
  const user = await sendRequestWithToken(ACCOUNT_API_URL, "GET");
  return user;
}

export async function updateUser(user) {
  const newUser = await sendRequestWithToken(ACCOUNT_API_URL, "PATCH", user);
  return newUser;
}

async function sendRequestWithToken(url, method, data) {
  const headers = new Headers();
  
  const token = localStorage.getItem(TOKEN_NAME);
  headers.set('Authorization', `Bearer ${token}`);

  if (data) {
    headers.set('Content-Type', 'application/json');
  }

  const requestOptions = {
    method: method,
    headers: headers,
    body: data ? JSON.stringify(data) : undefined
  };

  const resultFetch = await fetch(url, requestOptions);
  if (resultFetch.ok) {
    try {
      const result = await resultFetch.json();
      return result;
    }
    catch {
      return;
    }
  }
  else {
    errorRequest(resultFetch.status);
  }
}

function errorRequest(status) {
  if (status === 401) {
    window.location.href = BASE_URL;
    
  }
}


