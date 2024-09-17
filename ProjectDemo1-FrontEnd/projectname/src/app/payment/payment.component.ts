import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Router } from '@angular/router';
import { Booking } from '../models/room.model';
@Component({
  selector: 'app-payment',
  templateUrl: './payment.component.html',
  styleUrl: './payment.component.css'
})
export class PaymentComponent implements  OnInit{
  bookings: Booking[] = [];  // Array to hold all bookings
  filteredBookings: Booking[] = [];  // Array to hold filtered bookings
  loading: boolean = false;  // Loading state
  error: string | null = null; // Error state
  searchTerm: string = ''; // Search term for filtering

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.fetchBookings();
  }

  fetchBookings(): void {
    this.loading = true;
    this.error = null;

    this.http.get<Booking[]>('http://localhost:5046/api/Booking') // Replace with your API URL
      .subscribe(
        data => {
          this.bookings = data;
          this.filteredBookings = data; // Initialize filtered bookings with all data
          this.loading = false;
        },
        error => {
          this.error = 'Error fetching bookings. Please try again later.';
          this.loading = false;
          console.error('Error fetching bookings', error);
        }
      );
  }

  deleteBooking(bookingId: number): void {
    if (confirm('Are you sure you want to delete this booking?')) {
      this.http.delete(`http://localhost:5046/api/Booking/${bookingId}`) // Replace with your API URL
        .subscribe(
          () => {
            this.bookings = this.bookings.filter(booking => booking.id !== bookingId);
            this.filteredBookings = this.filteredBookings.filter(booking => booking.id !== bookingId);
            alert('Booking deleted successfully.');
          },
          error => {
            this.error = 'Error deleting booking. Please try again later.';
            console.error('Error deleting booking', error);
          }
        );
    }
  }

  onSearchChange(searchValue: string): void {
    this.searchTerm = searchValue.trim().toLowerCase();
    this.filteredBookings = this.bookings.filter(booking =>
      booking.userEmail.toLowerCase().includes(this.searchTerm)
    );
  }

}
