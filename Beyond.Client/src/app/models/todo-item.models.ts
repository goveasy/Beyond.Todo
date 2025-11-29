export interface ProgressionDto {
  id: number;
  date: string;
  percent: number;
}

export interface TodoItemDto {
  id: number;
  title: string;
  description: string;
  category: string;
  isCompleted: boolean;
  cumulativePercent: number;
  progressions: ProgressionDto[];
}

export interface TodoItemActionState {
  registeringProgression: boolean;
  updatingDescription: boolean;
  deleting: boolean;
  refreshing: boolean;
}

export interface CreateTodoItemRequest {
  title: string;
  description: string;
  category: string;
}

export interface RegisterProgressionRequest {
  date: string | Date;
  percent: number;
}

export interface UpdateDescriptionRequest {
  newDescription: string;
}
