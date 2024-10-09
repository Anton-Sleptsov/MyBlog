import { useState } from "react";
import { getToken } from "../../../services/repositories/accountRepository";
import { PROFILE_URL } from "../../../services/appConstants";

export default function Login() {
  const [login, setLogin] = useState();
  const [password, setPassword] = useState();

  async function setToken() {
    await getToken(login, password);
    window.location.href = PROFILE_URL;
  }

  return( <div>
    <h1>Вход</h1>
    <label >
      Эл. почта
      <input type="text" onChange={e => setLogin(e.target.value)}/>
    </label>
    <label >
      Пароль
      <input type="password" onChange={e => setPassword(e.target.value)}/>
    </label>
    <button className="btn btn-primary" onClick={setToken}>Получить токен</button>
  </div>);
}
