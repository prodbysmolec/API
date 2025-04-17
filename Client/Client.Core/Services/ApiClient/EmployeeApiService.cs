using System;
using API.Features.Employees.Models.DTOs;
using Artikelsystem.Shared;
using Artikelsystem.Shared.Constants;
using Artikelsystem.Shared.DTOs;
using Artikelsystem.Shared.DTOs.Employee.Response;

namespace Client.Core.Services.ApiClient;

    public class EmployeeApiService
    {
        private readonly HttpClientBase _httpClient;

        public EmployeeApiService(HttpClientBase httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PagedResultDTO<EmployeeDto>> GetAllEmployeesAsync(GetAllEmployeesRequest request)
        {
            return await _httpClient.PostAsync<GetAllEmployeesRequest, PagedResultDTO<EmployeeDto>>(
                ApiRoutes.Employee.GetAllEmployees, request);
        }
        
        public async Task<GetEmployeeResponse> GetEmployeeByIdAsync(int id)
        {
            var endpoint = ApiRoutes.Employee.GetEmployeeById.Replace("{id}", id.ToString());
            return await _httpClient.GetAsync<GetEmployeeResponse>(endpoint);
        }
        
        public async Task<ApiResponseDTO> CreateEmployeeAsync(CreateEmployeeRequest request)
        {
            return await _httpClient.PostAsync<CreateEmployeeRequest, ApiResponseDTO>(
                ApiRoutes.Employee.CreateEmployee, request);
        }
        
        public async Task<ApiResponseDTO> UpdateEmployeeAsync(int id, UpdateEmployeeRequest request)
        {
            var endpoint = ApiRoutes.Employee.UpdateEmployee.Replace("{id}", id.ToString());
            return await _httpClient.PutAsync<UpdateEmployeeRequest, ApiResponseDTO>(endpoint, request);
        }
    }