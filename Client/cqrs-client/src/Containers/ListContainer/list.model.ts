export interface Post {
    postId: string;
    author: string;
    datePosted: string;
    message: string;
    version: number;
    error?: string;
}

// export interface FormCallback (formData: { author: string; message: string }, post?: Post) => void;
