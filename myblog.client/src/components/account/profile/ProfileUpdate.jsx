import { useState } from "react";
import { updateUser } from "../../../services/repositories/accountRepository.js";
import ImageComponent from "../../ImageComponent.jsx";
import ImageUploader from "../../ImageUploader.jsx";

export default function ProfileUpdate({user, updateAction}) {
  const [userName, setUserName] = useState(user.name);
  const [userEmail, setUserEmail] = useState(user.email);
  const [userDescription, setUserDescription] = useState(user.description);
  const [userPhoto, setUserPhoto] = useState(user.photo);

  function sendData(){
    const newUser = {
      id: user.id,
      name: userName,
      email: userEmail,
      description: userDescription,
      photo: userPhoto,
    }

    updateUser(newUser);
    updateAction(newUser);
  }

  return (
    <div style={{display: "flex", flexDirection: "column"}}>
      <h2>Профиль пользователя "{user.name}"</h2>
      <label>
        Имя: <input type="text" defaultValue={userName} onChange={e => setUserName(e.target.value)} />
      </label>
      <label>
        Почта: <input type="text" defaultValue={userEmail} onChange={e => setUserEmail(e.target.value)} />
      </label>
      <label>
        Описание: <textarea type="text" defaultValue={userDescription} onChange={e => setUserDescription(e.target.value)} />
      </label>
      <ImageUploader byteImageAction={bytes => setUserPhoto(bytes)} />
      <ImageComponent byteArray={userPhoto} alt="Avatar" />
      <button onClick={sendData}>Изменить</button>
    </div>
  );
}
