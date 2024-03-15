import React from "react";
import logo from "./logo.svg";
import "./App.scss";
import ListContainer from "./Containers/ListContainer/ListContainer";

function App() {
  return (
    <div className="App p-5">
      <header>
        <h1>CQRS, event sourcing</h1>
      </header>
      <h5>Entries list</h5>
      <ListContainer />
    </div>
  );
}

export default App;
