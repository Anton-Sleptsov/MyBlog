import { useEffect, useState } from "react";
import { getCurrentUser } from "../../../services/repositories/accountRepository";
import ImageComponent from "../../ImageComponent.jsx";
import ModalButton from "../../ModalButton.jsx";
import ProfileUpdate from "./ProfileUpdate.jsx";

export default function Profile() {
  const [user, setUser] = useState({
    id: "",
    name: "",
    email: "",
    description: "",
    photo: [],
  });

  useEffect(() => {
    async function fetchUser() {
      const data = await getCurrentUser();
      setUser(data);
    }
    fetchUser();
  }, []);

  function updateCurrentUser(newUser) {
    setUser(newUser);
  };

  return (
    <div>
      <h2>Профиль</h2>
      <p>ID: {user.id}</p>
      <p>Имя: {user.name}</p>
      <p>Почта: {user.email}</p>
      <p>Описание: {user.description}</p>
      <ImageComponent byteArray={user.photo} alt="Avatar" />
      <ModalButton modalContent={<ProfileUpdate user={user} updateAction={updateCurrentUser}/>} title={"Редактирование профиля"} btnName={"Редактировать профиль"}/>
    </div>
  );
}
