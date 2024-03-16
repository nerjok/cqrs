// MyForm.tsx
import axios from 'axios';
import React, { useState, useEffect } from 'react';

interface MyFormProps {
    id: string;
}

const AuditTrail: React.FC<MyFormProps> = ({ id }) => {
    const [entries, setEntries] = useState<any[]>([]);

    const fetchEntries = () => {
        axios
            .get<any, { data: any[] }>(
                `http://localhost:5010/api/v1/NewPost/${id}/events`,
                {
                    params: {
                        ID: 12345,
                    },
                },
            )
            .then((response) => {
                console.log('[auditTrails ]', response.data);

                setEntries(response.data);
            });
    };
    useEffect(() => {
        fetchEntries();
        // setFormData(formValues ?? { author: '', message: '' });
    }, [id]);

    return (
        <div className='mt-5'>
            <figure>
                <figcaption className='table-list-caption'>
                    Audit trail
                </figcaption>
                {entries.map((data, index) => (
                    <dl key={index}>
                        <div>
                            <dt>Event</dt>
                            <dd style={{maxWidth: '300px', wordBreak: 'break-all'}}>{JSON.stringify(data)}</dd>
                        </div>
                    </dl>
                ))}
            </figure>
        </div>
    );
};

export default AuditTrail;
