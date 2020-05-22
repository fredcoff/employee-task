import { Component, TemplateRef } from '@angular/core';
import {
    TasksClient, CreateTaskCommand, TaskDto, UpdateTaskCommand,
    TodosVm, EmployeesClient, EmployeeDto, CreateEmployeeCommand, UpdateEmployeeCommand,
    UpdateTaskDetailCommand,
    TaskState
} from '../employeetask-api';
import { faPlus, faEllipsisH } from '@fortawesome/free-solid-svg-icons';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';

@Component({
    selector: 'app-todo-component',
    templateUrl: './todo.component.html',
    styleUrls: ['./todo.component.css']
})
export class TodoComponent {

    debug = false;

    vm: TodosVm;

    selectedEmployee: EmployeeDto;
    selectedTask: TaskDto;

    newEmployeeEditor: any = {};
    employeeOptionsEditor: any = {};
    taskDetailsEditor: any = {};

    newEmployeeModalRef: BsModalRef;
    employeeOptionsModalRef: BsModalRef;
    deleteEmployeeModalRef: BsModalRef;
    taskDetailsModalRef: BsModalRef;

    faPlus = faPlus;
    faEllipsisH = faEllipsisH;

    TaskState = TaskState;

    constructor(private employeesClient: EmployeesClient, private tasksClient: TasksClient, private modalService: BsModalService) {
        employeesClient.get().subscribe(
            result => {
                this.vm = result;
                if (this.vm.employees.length) {
                    this.selectedEmployee = this.vm.employees[0];
                }
            },
            error => console.error(error)
        );
    }

    // Employees
    remainingTasks(employee: EmployeeDto): number {
        return employee.tasks.filter(t => t.state !== TaskState.Closed).length;
    }

    showNewEmployeeModal(template: TemplateRef<any>): void {
        this.newEmployeeModalRef = this.modalService.show(template);
        setTimeout(() => document.getElementById("title").focus(), 250);
    }

    newEmployeeCancelled(): void {
        this.newEmployeeModalRef.hide();
        this.newEmployeeEditor = {};
    }

    addEmployee(): void {
        let employee = EmployeeDto.fromJS({
            id: 0,
            name: this.newEmployeeEditor.name,
            tasks: [],
        });

        this.employeesClient.create(<CreateEmployeeCommand>{ name: this.newEmployeeEditor.name }).subscribe(
            result => {
                employee.id = result;
                this.vm.employees.push(employee);
                this.selectedEmployee = employee;
                this.newEmployeeModalRef.hide();
                this.newEmployeeEditor = {};
            },
            error => {
                let errors = JSON.parse(error.response);

                if (errors && errors.Title) {
                    this.newEmployeeEditor.error = errors.Title[0];
                }

                setTimeout(() => document.getElementById("title").focus(), 250);
            }
        );
    }

    showEmployeeOptionsModal(template: TemplateRef<any>) {
        this.employeeOptionsEditor = {
            id: this.selectedEmployee.id,
            name: this.selectedEmployee.name,
        };

        this.employeeOptionsModalRef = this.modalService.show(template);
    }

    updateEmployeeOptions() {
        this.employeesClient.update(this.selectedEmployee.id, UpdateEmployeeCommand.fromJS(this.employeeOptionsEditor))
            .subscribe(
                () => {
                    this.selectedEmployee.name = this.employeeOptionsEditor.name,
                        this.employeeOptionsModalRef.hide();
                    this.employeeOptionsEditor = {};
                },
                error => console.error(error)
            );
    }

    confirmDeleteEmployee(template: TemplateRef<any>) {
        this.employeeOptionsModalRef.hide();
        this.deleteEmployeeModalRef = this.modalService.show(template);
    }

    deleteEmployeeConfirmed(): void {
        this.employeesClient.delete(this.selectedEmployee.id).subscribe(
            () => {
                this.deleteEmployeeModalRef.hide();
                this.vm.employees = this.vm.employees.filter(t => t.id != this.selectedEmployee.id)
                this.selectedEmployee = this.vm.employees.length ? this.vm.employees[0] : null;
            },
            error => console.error(error)
        );
    }

    // Tasks

    showTaskDetailsModal(template: TemplateRef<any>, task: TaskDto): void {
        this.selectedTask = task;
        this.taskDetailsEditor = {
            ...this.selectedTask
        };

        this.taskDetailsModalRef = this.modalService.show(template);
    }

    updateTaskDetails(): void {
        this.tasksClient.updateTaskDetails(this.selectedTask.id, UpdateTaskDetailCommand.fromJS(this.taskDetailsEditor))
            .subscribe(
                () => {
                    if (this.selectedTask.employeeId != this.taskDetailsEditor.employeeId) {
                        this.selectedEmployee.tasks = this.selectedEmployee.tasks.filter(i => i.id != this.selectedTask.id)
                        let employeeIndex = this.vm.employees.findIndex(l => l.id == this.taskDetailsEditor.employeeId);
                        this.selectedTask.employeeId = this.taskDetailsEditor.employeeId;
                        this.vm.employees[employeeIndex].tasks.push(this.selectedTask);
                    }

                    this.selectedTask.priority = this.taskDetailsEditor.priority;
                    this.selectedTask.description = this.taskDetailsEditor.note;
                    this.taskDetailsModalRef.hide();
                    this.taskDetailsEditor = {};
                },
                error => console.error(error)
            );
    }

    addTask() {
        let task = TaskDto.fromJS({
            id: 0,
            employeeId: this.selectedEmployee.id,
            priority: this.vm.priorityLevels[0].value,
            title: '',
            state: TaskState.New
        });

        this.selectedEmployee.tasks.push(task);
        let index = this.selectedEmployee.tasks.length - 1;
        this.editTask(task, 'itemTitle' + index);
    }

    editTask(task: TaskDto, inputId: string): void {
        this.selectedTask = task;
        setTimeout(() => document.getElementById(inputId).focus(), 100);
    }

    updateTask(task: TaskDto, pressedEnter: boolean = false): void {
        let isNewTask = task.id == 0;

        if (!task.title.trim()) {
            this.deleteTask(task);
            return;
        }

        if (task.id == 0) {
            this.tasksClient.create(CreateTaskCommand.fromJS({ ...task, employeeId: this.selectedEmployee.id }))
                .subscribe(
                    result => {
                        task.id = result;
                    },
                    error => console.error(error)
                );
        } else {
            this.tasksClient.update(task.id, UpdateTaskCommand.fromJS(task))
                .subscribe(
                    () => console.log('Update succeeded.'),
                    error => console.error(error)
                );
        }

        this.selectedTask = null;

        if (isNewTask && pressedEnter) {
            this.addTask();
        }
    }

    // Delete task
    deleteTask(task: TaskDto) {
        if (this.taskDetailsModalRef) {
            this.taskDetailsModalRef.hide();
        }

        if (task.id == 0) {
            let itemIndex = this.selectedEmployee.tasks.indexOf(this.selectedTask);
            this.selectedEmployee.tasks.splice(itemIndex, 1);
        } else {
            this.tasksClient.delete(task.id).subscribe(
                () => this.selectedEmployee.tasks = this.selectedEmployee.tasks.filter(t => t.id != task.id),
                error => console.error(error)
            );
        }
    }
}
