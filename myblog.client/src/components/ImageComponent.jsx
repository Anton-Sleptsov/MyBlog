export default function ImageComponent({byteArray, alt}) {
if (!byteArray) {
  return <></>;
}

  const base64String = btoa(String.fromCharCode(...byteArray));
  const imageUrl = `data:image/jpeg;base64,${base64String}`;

  return <img src={imageUrl} alt={alt} />;
}