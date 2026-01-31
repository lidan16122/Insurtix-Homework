export interface Book {
  category: string;
  cover?: string | null;
  isbn: string;
  title: string;
  lang: string;
  authors: string[];
  year: number;
  price: number;
}
