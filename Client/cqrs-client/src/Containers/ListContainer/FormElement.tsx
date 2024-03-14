// MyForm.tsx
import React, { useState, ChangeEvent, FormEvent, useEffect } from "react";
import { Post } from "./list.model";

interface MyFormProps {
  onSubmit: (
    formData: { author: string; message: string },
    post?: Post
  ) => void;
  formValues?: Post;
}

const EntryForm: React.FC<MyFormProps> = ({ onSubmit, formValues }) => {
  const [formData, setFormData] = useState({
    author: formValues?.author ?? "",
    message: formValues?.message ?? "",
  });

  useEffect(() => {
    setFormData(formValues ?? { author: "", message: "" });
  }, [formValues]);

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value,
    });
  };

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    onSubmit(formData, formValues);

    // If you want to reset the form after submission
    setFormData({
      author: "",
      message: "",
    });
  };

  return (
    <form onSubmit={handleSubmit}>
      <div className="form-group">
        <label htmlFor="author">Author</label>
        <input
          id="author"
          type="text"
          name="author"
          value={formData.author}
          onChange={handleInputChange}
        />
      </div>

      <div className="form-group">
        <label htmlFor="message">Message</label>
        <input
          id="message"
          type="text"
          name="message"
          value={formData.message}
          onChange={handleInputChange}
        />
      </div>

      <button type="submit">Submit</button>
    </form>
  );
};

export default EntryForm;
