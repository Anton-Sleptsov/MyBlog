export default function ImageUploader({ byteImageAction }) {
  const handleFileChange = (event) => {
    const file = event.target.files[0];

    if (file) {
      const reader = new FileReader();

      reader.onload = (e) => {
        const byteArray = new Uint8Array(e.target.result);
        byteImageAction(byteArray);
      };

      reader.readAsArrayBuffer(file);
    }
  };

  return (
    <div>
      <input type="file" accept="image/*" onChange={handleFileChange}/>
    </div>
  )
}
