import { Component,OnInit  } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http'; // Import HttpClient
import { Observable } from 'rxjs'; // Import Observable
import { HttpHeaders } from '@angular/common/http';
import { ServiceService } from '../services/service.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrl: './users.component.css'
})
export class UsersComponent implements OnInit {
  users: any[] = []; // Define the type according to your data structure
  private apiUrl = 'Users/GetUsers'; // Your API URL

  constructor(private http: HttpClient,private router:Router,private service:ServiceService

    
  ) { }

 
  ngOnInit(): void {
    this.loadUsers();
  }

  loadUsers(): void {
    const headers = new HttpHeaders({
      'Authorization': `Bearer ${sessionStorage.getItem('authToken')}`
    });
this.service.get(this.apiUrl).subscribe(
  response => {
    this.users = response.users; // Adjust according to your response structure
  },
  error => {
    console.error('Error fetching users', error);
  }
);
}

  //   this.http.get<any>(this.apiUrl, { headers }).subscribe(
  //     response => {
  //       this.users = response.users; // Adjust according to your response structure
  //     },
  //     error => {
  //       console.error('Error fetching users', error);
  //     }
  //   );
  // }

  editUser(id: number): void {
    console.log('Navigating to edit user with ID:', id); // For debugging
    this.router.navigate(['/dashboard/edituser', id]); // Correct route navigation
  }

  deleteUser(id: number): void {
    if(confirm("are you want to delete the user?"))
    this.service.delete(`Users/DeleteUser/${id}`)
    .subscribe(
      () => this.loadUsers(),
      
    );
  }
  
}
