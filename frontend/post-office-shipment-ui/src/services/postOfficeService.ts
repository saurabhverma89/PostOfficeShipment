import { apiClient } from "../api/axiosClient";
import type { PostOffice } from "../types/shipment";

export interface CreatePostOfficeRequest {
  zipCode: string;
  name: string;
  address?: string;
}

export interface UpdatePostOfficeRequest {
  zipCode: string;
  name: string;
  address?: string;
}

export async function getPostOffices(): Promise<PostOffice[]> {
  const response = await apiClient.get<PostOffice[]>("/postoffices");

  return response.data;
}

export async function getPostOfficeById(id: number): Promise<PostOffice> {
  const response = await apiClient.get<PostOffice>(`/postoffices/${id}`);

  return response.data;
}

export async function createPostOffice(request: CreatePostOfficeRequest): Promise<PostOffice> {
  const response = await apiClient.post<PostOffice>("/postoffices", request);

  return response.data;
}

export async function updatePostOffice(id: number, request: UpdatePostOfficeRequest): Promise<PostOffice> {
  const response = await apiClient.put<PostOffice>(`/postoffices/${id}`, request);

  return response.data;
}

export async function deletePostOffice(id: number): Promise<void> {
  await apiClient.delete(`/postoffices/${id}`);
}