import React, { useEffect, useState } from 'react';
import axios from 'axios';
import EntryForm from './FormElement';
import { Post } from './list.model';
import { Modal, Tab, Tabs } from '@sebgroup/green-react';
import AuditTrail from './AuditTrail';

function ListContainer() {
    const [entries, setEntries] = useState<Post[]>([]);
    const [createMode, setCreateMode] = useState<{
        data?: Post;
        isActive: boolean;
    }>({ isActive: false });

    const fetchEntries = () => {
        axios
            .get<any, { data: { posts: Post[] } }>(
                'http://localhost:5011/api/v1/PostLookup',
                {
                    params: {
                        ID: 12345,
                    },
                },
            )
            .then((response) => {
                const data = response?.data?.posts ?? [];
                setEntries(data);
            })
            .catch((error) => {
                console.error('[fetch entires error]', error);
            });
    };

    useEffect(() => {
        fetchEntries();
    }, []);

    const createEntry = (data: any) => {
        axios
            .post('http://localhost:5010/api/v1/NewPost', data)
            .then((response: any) => {
                console.log('[[ succesfullt saved]]', response);
                setCreateMode({ isActive: false });
                setTimeout(() => {
                    fetchEntries();
                }, 500);
            })
            .catch((error) => {
                console.error('[create message error]', error);
            });
    };

    const updateEntry = (data: Post) => {
        axios
            .put(`http://localhost:5010/api/v1/NewPost/${data.postId}`, {
                id: data.postId,
                message: data.message,
                version: data.version,
            })
            .then((response: any) => {
                setCreateMode({ isActive: false });
                setTimeout(() => {
                    fetchEntries();
                }, 500);
            })
            .catch((error) => {
                console.error('[update message error]', error);
                setCreateMode({
                    isActive: true,
                    data: { ...data, error: 'smthing went wrong' },
                });
            });
    };

    const updateMessage = (data: Post) => {
        setCreateMode({ isActive: true, data });
    };

    const deleteEntry = (post: Post) => {
        axios
            .delete(
                `http://localhost:5010/api/v1/NewPost/remove?id=${post.postId}`,
                {
                    data: {
                        id: post.postId,
                        username: post.author,
                    },
                },
            )
            .then((response: any) => {
                console.log('[[ succesfullt saved]]', response);
                setCreateMode({ isActive: false });
                setTimeout(() => {
                    fetchEntries();
                }, 500);
            });
    };

    const submitForm = (
        formData: { author: string; message: string },
        post?: Post,
    ) => {
        if (post) {
            updateEntry({ ...post, ...formData });
        } else {
            createEntry(formData);
        }
    };

    const createNewEntry = () => {
        setCreateMode({ isActive: !createMode.isActive });
    };

    const closeModal = () => {
        setCreateMode({ isActive: false });
    };

    return (
        <div>
            <button onClick={fetchEntries}>Fetch Entries</button>
            <button onClick={createNewEntry}>Create New</button>
            <table className='table'>
                <thead>
                    <tr>
                        <th scope='col'>#</th>
                        <th scope='col'>Author</th>
                        <th scope='col'>Message</th>
                        <th scope='col'>Timestamp</th>
                        <th scope='col'>Action</th>
                    </tr>
                </thead>
                <tbody>
                    {entries.map((element, i) => {
                        return (
                            <tr key={'list_' + element.postId}>
                                <th scope='row'>#</th>
                                <td>{element.author}</td>
                                <td>{element.message}</td>
                                <td>{element.datePosted}</td>
                                <td>
                                    <button
                                        onClick={() => updateMessage(element)}
                                    >
                                        Update
                                    </button>
                                    <button
                                        onClick={() => deleteEntry(element)}
                                    >
                                        Delete
                                    </button>
                                </td>
                            </tr>
                        );
                    })}
                </tbody>
            </table>
            {createMode.isActive ? (
                <Modal
                    confirm='OK'
                    isOpen={createMode.isActive}
                    dismiss='Nope'
                    header='Entry header'
                    onClose={closeModal}
                    onConfirm={closeModal}
                    size='sm'
                    type='slideout'
                >
                    {createMode.data?.postId ? (
                        <Tabs>
                            <Tab title={'Details form'}>
                                <EntryForm
                                    onSubmit={submitForm}
                                    formValues={createMode.data}
                                />
                            </Tab>
                            <Tab title={'Audit trail'}>
                                <AuditTrail id={createMode.data?.postId} />
                            </Tab>
                        </Tabs>
                    ) : (
                        <EntryForm onSubmit={submitForm} />
                    )}
                </Modal>
            ) : null}
        </div>
    );
}

export default ListContainer;
