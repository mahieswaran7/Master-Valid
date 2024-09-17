export interface Room {
  id: number;
  roomNumber: number;
  roomType: string;
  price: number;
  isAvailable: boolean;
  imagePath?: string;
  bathRoom:string; 
  hall:string; 
  editMode?: boolean; 
  rating :number;
  location:string; 
  description:string,
  amenities?: string;
  isBoooked?:boolean;
  bedRoom?:string;
}
export class RoomService {
  id?: number;
  staffName?: string;
  email?: string;
  contact?: number;
  address?: string;
  rating?: number;
  isAvailable?: string;
  aadhar?: number;
  imagePath?: string;
  joinedDate?: Date;
  imageData?: File;
}

export interface Feedback {
  id?: number;
  name: string;
  email: string;
  phoneNumber: string;
  message: string;
}
export interface User {
  email?: string; // Use optional if email might not be present
  role?: string;
}

export interface LoginResponse {
  token?: string;
  user?: User;
}
export interface Booking {
  id: number;
  roomId: number;
  userEmail: string;
  bookingDate: string; // Using string to handle Date format from API response
  isConfirmed: boolean;
  roomNumber: string;
  roomType: string;
  roomPrice: number;
}