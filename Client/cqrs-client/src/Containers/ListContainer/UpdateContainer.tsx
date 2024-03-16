import React, { useEffect, useRef } from 'react';
import {
    HubConnection,
    HubConnectionBuilder,
    LogLevel,
} from '@microsoft/signalr';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function UpdateContainer() {
    const notify = () =>
        toast.success('🦄 Data has changed!!!', {
            position: 'top-right',
            autoClose: 5000,
            hideProgressBar: false,
            closeOnClick: true,
            pauseOnHover: true,
            draggable: true,
            progress: undefined,
            theme: 'light',
        });

    let connectionRef = useRef<HubConnection>();

    
    useEffect(() => {
        console.log('[hookCalled]');
        
        let connection = connectionRef.current;
        connection?.stop();
        if (!connection) {
            connection = new HubConnectionBuilder()
                .withUrl('http://localhost:5011/chathub')
                .configureLogging(LogLevel.Information)
                .build();

            connection
                .start()
                .then(() => {
                    console.log('Connection established.');
                })
                .catch((err) => {
                    console.error('conectionError ', err.toString());
                });

            connection.on('SendMessage', (msg) => {
                console.log('[ userSpecificErrorReceived ]', msg);
                setTimeout(() => {
                    notify();
                }, 1000);
            });
        }
    }, []);
    useEffect(() => {
        console.log('[iam called twice]');
        
    }, [])

    return (

            <ToastContainer
                position='top-right'
                autoClose={5000}
                hideProgressBar={false}
                newestOnTop={false}
                closeOnClick
                rtl={false}
                pauseOnFocusLoss
                draggable
                pauseOnHover
                theme='light'
            />
    );
}

export default UpdateContainer;
