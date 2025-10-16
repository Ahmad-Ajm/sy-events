// تعليق: مكون منتدى النقاش - عرض وإضافة التعليقات
import { Component, Input, OnInit, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';

interface Discussion {
  id: string;
  userId: string;
  userName: string;
  userProfileImage: string;
  message: string;
  creationTime: string;
  replies: Discussion[];
  repliesCount: number;
}

@Component({
  selector: 'app-discussion',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  template: `
    <!-- تعليق: منتدى النقاش للفعالية -->
    <div class="discussion-container">
      <div class="card">
        <div class="card-header">
          <h5 class="mb-0">
            <i class="fas fa-comments me-2"></i>
            النقاش ({{ discussions().length }})
          </h5>
        </div>
        <div class="card-body">
          
          <!-- نموذج إضافة تعليق -->
          <form [formGroup]="commentForm" (ngSubmit)="addComment()" class="mb-4">
            <div class="mb-3">
              <textarea class="form-control" 
                        formControlName="message" 
                        rows="3" 
                        placeholder="اكتب تعليقك هنا..."></textarea>
            </div>
            <button type="submit" class="btn btn-primary" [disabled]="commentForm.invalid || posting">
              <i class="fas fa-paper-plane me-2"></i>
              @if (posting) { جاري النشر... } @else { نشر التعليق }
            </button>
          </form>

          <!-- قائمة التعليقات -->
          @if (discussions().length > 0) {
            @for (comment of discussions(); track comment.id) {
              <div class="comment-item mb-3">
                <div class="d-flex">
                  <img [src]="comment.userProfileImage || '/assets/default-avatar.png'" 
                       class="comment-avatar me-3">
                  <div class="flex-grow-1">
                    <div class="d-flex justify-content-between align-items-start mb-2">
                      <div>
                        <strong>{{ comment.userName }}</strong>
                        <small class="text-muted ms-2">{{ comment.creationTime | date:'short' }}</small>
                      </div>
                      <button class="btn btn-sm btn-link text-danger" (click)="deleteComment(comment.id)">
                        <i class="fas fa-trash"></i>
                      </button>
                    </div>
                    <p class="mb-2">{{ comment.message }}</p>
                    
                    <!-- زر الرد -->
                    <button class="btn btn-sm btn-link" (click)="toggleReply(comment.id)">
                      <i class="fas fa-reply me-1"></i>
                      رد ({{ comment.repliesCount }})
                    </button>

                    <!-- الردود -->
                    @if (comment.replies && comment.replies.length > 0) {
                      <div class="replies mt-3">
                        @for (reply of comment.replies; track reply.id) {
                          <div class="reply-item d-flex mb-2">
                            <img [src]="reply.userProfileImage || '/assets/default-avatar.png'" 
                                 class="reply-avatar me-2">
                            <div class="flex-grow-1">
                              <div><strong>{{ reply.userName }}</strong></div>
                              <p class="mb-0 small">{{ reply.message }}</p>
                            </div>
                          </div>
                        }
                      </div>
                    }

                    <!-- نموذج الرد -->
                    @if (replyingTo() === comment.id) {
                      <div class="reply-form mt-3">
                        <form [formGroup]="replyForm" (ngSubmit)="submitReply(comment.id)">
                          <div class="input-group">
                            <input type="text" class="form-control" formControlName="message" 
                                   placeholder="اكتب ردك...">
                            <button type="submit" class="btn btn-primary" [disabled]="replyForm.invalid">
                              <i class="fas fa-paper-plane"></i>
                            </button>
                            <button type="button" class="btn btn-secondary" (click)="cancelReply()">
                              <i class="fas fa-times"></i>
                            </button>
                          </div>
                        </form>
                      </div>
                    }
                  </div>
                </div>
              </div>
            }
          } @else {
            <div class="text-center text-muted py-5">
              <i class="fas fa-comments fa-4x mb-3 opacity-25"></i>
              <p>لا توجد تعليقات بعد</p>
              <p class="small">كن أول من يعلق!</p>
            </div>
          }
        </div>
      </div>
    </div>
  `,
  styles: [`
    .comment-avatar {
      width: 50px;
      height: 50px;
      border-radius: 50%;
      object-fit: cover;
    }
    
    .reply-avatar {
      width: 35px;
      height: 35px;
      border-radius: 50%;
      object-fit: cover;
    }
    
    .comment-item {
      padding: 1rem;
      border-radius: 8px;
      background-color: #f8f9fa;
    }
    
    .replies {
      padding-left: 2rem;
      border-left: 2px solid #dee2e6;
    }
    
    .reply-item {
      padding: 0.5rem;
      background-color: white;
      border-radius: 4px;
    }
  `]
})
export class DiscussionComponent implements OnInit {
  @Input() eventId: string = '';
  
  discussions = signal<Discussion[]>([]);
  replyingTo = signal<string | null>(null);
  posting = false;

  commentForm: FormGroup;
  replyForm: FormGroup;

  constructor(
    private readonly fb: FormBuilder,
    private readonly http: HttpClient
  ) {
    this.commentForm = this.fb.group({
      message: ['', [Validators.required, Validators.minLength(3)]]
    });
    
    this.replyForm = this.fb.group({
      message: ['', [Validators.required]]
    });
  }

  ngOnInit(): void {
    this.loadDiscussions();
  }

  loadDiscussions(): void {
    if (!this.eventId) return;
    
    this.http.get<Discussion[]>(`/api/app/event-discussion/event/${this.eventId}`).subscribe({
      next: (data) => this.discussions.set(data),
      error: (err) => console.error('فشل تحميل التعليقات', err)
    });
  }

  addComment(): void {
    if (this.commentForm.invalid) return;

    this.posting = true;
    const payload = {
      eventId: this.eventId,
      message: this.commentForm.value.message
    };

    this.http.post('/api/app/event-discussion', payload).subscribe({
      next: () => {
        this.posting = false;
        this.commentForm.reset();
        this.loadDiscussions();
      },
      error: (err) => {
        this.posting = false;
        console.error('فشل نشر التعليق', err);
      }
    });
  }

  toggleReply(commentId: string): void {
    this.replyingTo.set(this.replyingTo() === commentId ? null : commentId);
  }

  submitReply(parentId: string): void {
    if (this.replyForm.invalid) return;

    const payload = {
      eventId: this.eventId,
      message: this.replyForm.value.message,
      parentId: parentId
    };

    this.http.post('/api/app/event-discussion', payload).subscribe({
      next: () => {
        this.replyForm.reset();
        this.replyingTo.set(null);
        this.loadDiscussions();
      },
      error: (err) => console.error('فشل نشر الرد', err)
    });
  }

  cancelReply(): void {
    this.replyingTo.set(null);
    this.replyForm.reset();
  }

  deleteComment(id: string): void {
    if (!confirm('هل أنت متأكد من حذف هذا التعليق؟')) return;

    this.http.delete(`/api/app/event-discussion/${id}`).subscribe({
      next: () => this.loadDiscussions(),
      error: (err) => console.error('فشل حذف التعليق', err)
    });
  }
}

