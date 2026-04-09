---
sidebar_position: 2
---

# Stakeholders

## Overview

BookWorm có nhiều stakeholders với các mối quan tâm khác nhau. Mục tiêu trang này là xác định:
- **Stakeholders chính** và mối quan tâm của họ  
- **Personas cụ thể** và hành vi của họ  
- **Đánh giá độ quan trọng** của mỗi stakeholder

---

## Bảng tổng quan Stakeholders

| Stakeholder | Vai trò |主点/ Hoff | Cách BookWorm giải quyết |
|-------------|---------|---------------------|-------------------------|
| **Nghiên cứu viên** | Researcher | Tìm kiếm nhanh, dùng dễ, đa ngôn ngữ | Tìm kiếm thông minh, giao diện SLV/ANH |
| **Phát triển viên** | Developer | API rõ ràng, tài liệu kỹ thuật | API Auto-doc, tài liệu đầy đủ |
| **Quản lý** | Manager | Quản lý quyền, quy trình nhóm | Hỗ trợ nhóm, quản lý quyền, báo cáo |
| **Nhà cung cấp dịch vụ** | Vendor | Tích hợp, mở rộng | API support, cấu hình linh hoạt |

---

## User Personas

### Persona 1: Researcher (Nghiên cứu viên)

**Tên đặt**: thu Dao  
**Vai trò**: Nhà nghiên cứu chính  
**Bối cảnh**: Công việc nghiên cứu AI và ML

#### Mô tả
Thu Dao quản lý 100+ tài liệu PDF về AI. Cô cần:
- Tìm tài liệu nhanh (&lt; 3P)
- Tổ chức theo chủ đề
- Đọc bằng tiếng Việt

#### Thống kê
- Tìm kiếm: 5-10 lần hàng ngày
- 100-500 PDF/năm
- Thời gian tìm 15-30 giây

#### Yêu cầu chính
```
├── Tìm kiếm nhanh < 3P
├── Giao diện SLV trực quan
├── Tự động phân loại
└── Tìm kiếm chính xác
```

**Matter nhất**:
- Tối ưu: Tìm kiếm, tổ chức
- Ưu tiên: Đơn giản, nhanh
- Quản lý: Lên tới 500 PDF

---

### Persona 2: Developer (Nhà phát triển)

**Tên đặt**: minh Phong  
**Vai trò**: Backend Developer  
**Bối cảnh**: Tích hợp APIs

#### Mô tả
Minh Phong build APIs BookWorm vào hệ thống bên trong. Anh cần:
- API đầy đủ
- Tài liệu kỹ thuật
- API ổn định, versioning

#### Thống kê
- API calls: 1-5 lần/ngày
- Cần tài liệu chi tiết

#### Matter nhất
```
├── Tài liệu API đầy đủ
├── API ổn định
├── Documentation tốt
└── Versioning rõ ràng
```

**Matter nhất**:
- Tối ưu: Tích hợp, tài liệu
- Ưu tiên: API dễ, document
- Quản lý: Versioning, SLV

---

### Persona 3: Team Lead (Quản lý)

**Tên đặt**: Hoan Nguyen  
**Vai trò**: Team Lead  
**Bối cảnh**: Quản lý nhóm

#### Mô tả
Hoan quản lý nhóm 3-5 người. Anh cần:
- Quản lý team collaboration
- Báo cáo, giám sát
- Kiểm soát quyền

#### Thống kê
- Team: 3-5 người
- Họp: Weekly reviews

#### Matter nhất
```
├── Group management
├── Báo cáo team
├── Quyền truy cập
└── Audit logs
```

**Matter nhất**:
- Tối ưu: Nhóm, báo cáo
- Ưu tiên: Quản lý team
- Quản lý: Quyền, logs

---

## Stakeholder Alignment Matrix

| Quality Goal | Stakeholder | Priority | Tác động |
|--------------|-------------|----------|---------|
| Tìm kiếm nhanh | Researcher | CRITICAL | Core value |
| Bilingual | All users | CRITICAL | Must-have |
| API Stable | Developer | HIGH | Trust |
| Group Support | Team Lead | MEDIUM | Enhancement |
| Security | All | HIGH | Trust |

---

*Để xem detailed requirements xem [Section 1.1 - Requirement](./Requirement.md)*
