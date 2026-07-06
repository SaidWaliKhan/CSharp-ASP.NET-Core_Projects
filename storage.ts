import { Todo } from './types.js';

const STORAGE_KEY = 'todos_ts';

export function loadTodos(): Todo[] {
  try {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (raw) {
      const parsed = JSON.parse(raw);
      return parsed.map((t: any) => ({
        ...t,
        createdAt: new Date(t.createdAt),
        updatedAt: new Date(t.updatedAt),
      }));
    }
  } catch {
    // ignore
  }
  // default seed data
  return [
    {
      id: Date.now().toString(36) + Math.random().toString(36).substring(2, 6),
      title: 'Welcome to TypeScript Todo!',
      description: 'Try editing, completing, or filtering todos.',
      completed: false,
      priority: 'medium' as const,
      category: 'Demo',
      dueDate: null,
      createdAt: new Date(),
      updatedAt: new Date(),
    },
    {
      id: Date.now().toString(36) + Math.random().toString(36).substring(2, 6),
      title: 'Add a new todo',
      description: 'Use the form above to add tasks.',
      completed: false,
      priority: 'high' as const,
      category: 'Getting started',
      dueDate: null,
      createdAt: new Date(),
      updatedAt: new Date(),
    },
    {
      id: Date.now().toString(36) + Math.random().toString(36).substring(2, 6),
      title: 'Mark me as done',
      description: 'Click the checkbox to complete me.',
      completed: true,
      priority: 'low' as const,
      category: 'Demo',
      dueDate: null,
      createdAt: new Date(),
      updatedAt: new Date(),
    },
  ];
}

export function saveTodos(todos: Todo[]): void {
  try {
    localStorage.setItem(STORAGE_KEY, JSON.stringify(todos));
  } catch {
    // ignore
  }
}