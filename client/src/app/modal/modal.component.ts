import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
})
export class ModalComponent {
  @Output() closeModal = new EventEmitter<void>();
  @Input() modalType: 'add' | 'edit' | '' = '';

  onClose(): void {
    this.closeModal.emit();
  }
}
