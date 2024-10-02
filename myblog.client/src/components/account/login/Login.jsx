import { useState } from "react";
import { getToken } from "../../../services/repositories/accountRepository";

export default function Login() {
  const [login, setLogin] = useState();
  const [password, setPassword] = useState();

  const showToken = () => {
    const token = getToken(login, password);
    console.log(token); 
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
    <button className="btn btn-primary" onClick={showToken}>Получить токен</button>
  </div>);
}
