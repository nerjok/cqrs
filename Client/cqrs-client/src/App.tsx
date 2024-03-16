import React from 'react';
import './App.scss';
import ListContainer from './Containers/ListContainer/ListContainer';

import 'react-toastify/dist/ReactToastify.css';
import UpdateContainer from './Containers/ListContainer/UpdateContainer';

function App() {

    return (
        <div className='App p-5'>
            <header>
                <h1>CQRS, event sourcing</h1>
            </header>
            <h5>Entries list</h5>
            <ListContainer />
            <UpdateContainer />
        </div>
    );
}

export default App;
