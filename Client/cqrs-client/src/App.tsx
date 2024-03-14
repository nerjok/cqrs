import React from "react";
import logo from "./logo.svg";
import "./App.scss";
import ListContainer from "./Containers/ListContainer/ListContainer";

function App() {
  return (
    <div className="App p-5">
      <header>CQRS, event sourcing</header>
      <ListContainer />
    </div>
  );
}

export default App;
