---
sidebar_position: 3
---

# Requirements

This document captures **functional** and **non-functional** requirements that influence the BookWorm architecture design.

---

## Functional Requirements (FR)

### 1. Document Management

#### FR-001: PDF Upload
**Requirement ID**: FR-DM-001  
**Title**: Tải lên tài liệu PDF  
**Status**: PROPOSED

**Description**:Hệ thống cho phép người dùng tải lên tài liệu PDF có khối lượng tối đa 10MB.

**Pre-conditions**: User đã đăng nhập.

**Post-conditions**: Tài liệu được lưu vào storage và hiển thị trong danh sách.

**Measure**: Upload thành công trong < 5 phút cho file 10MB.

**Acceptance Criteria**:
1. UC-DM-001.1 - Người dùng có thể upload tối đa 100 PDF đồng thời per endpoint.
2. UC-DM-001.2 - Upload progress được hiển thị đến user theo thời gian thực.
3. UC-DM-001.3 - Hệ thống xử lý file 10MB trong < 5 phút.
4. UC-DM-001.4 - Error message rõ ràng khi upload thất bại.

**Priority**: HIGH  
**Impact**: Core feature - không thể thiếu.

---

#### FR-002: Document Tagging & Organization
**Requirement ID**: FR-DM-002  
**Title**: Phân loại và gắn thẻ tài liệu  
**Status**: PROPOSED

**Description**: Người dùng có thể:
- Gắn thẻ cho tài liệu (hàng trăm thẻ/quy bị)
- Tạo nhóm chat tùy chỉnh
- Tổ chức theo thư mục phân cấp

**Acceptance Criteria**:
1. UC-DM-002.1 - Add thẻ trong < 1 giây.
2. UC-DM-002.2 - Hỗ trợ tối đa 500 thẻ/tài liệu.
3. UC-DM-002.3 - Tạo được 100+ thư mục phân cấp.
4. UC-DM-002.4 - Tìm kiếm theo tag thực thi trong < 1 giây.

**Priority**: HIGH  
**Impact**: Core feature - cần thiết cho user experience.

---

### 2. ML Integration

#### FR-003: Auto-tagging
**Requirement ID**: FR-ML-001  
**Title**: Tự động gắn thẻ  
**Status**: PROPOSED

**Description**: Hệ thống tự động:
- Phân tích nội dung PDF
- Đề xuất tự động tags
- Phân loại theo chủ đề

**Acceptance Criteria**:
1. UC-ML-001.1 - Độ chính xác tags tự động đạt 85%+.
2. UC-ML-001.2 - Xử lý 100 PDF trong < 20 phút.
3. UC-ML-001.3 - Hỗ trợ tối đa 50+ tags.
4. UC-ML-001.4 - User có thể edit/delete tags tự động.

**Priority**: MEDIUM  
**Impact**: Enhancement - cải thiện user experience.

---

### 3. Search Functionality

#### FR-004: Hybrid Search
**Requirement ID**: FR-SS-001  
**Title**: Tìm kiếm đa phương thức  
**Status**: PROPOSED

**Description**: Kết hợp tìm kiếm:
- Exact text match
- Semantic search
- Multi-language (EN/VN) support

**Acceptance Criteria**:
1. UC-SS-001.1 - Response < 3 giây.
2. UC-SS-001.2 - > 90% relevant results.
3. UC-SS-001.3 - Tìm kiếm cả tiếng Việt/Anh.
4. UC-SS-001.4 - Filter theo tag, date, category.

**Priority**: HIGH  
**Impact**: Core feature - phải có.

---

## Non-Functional Requirements (NFR)

### 1. Performance

#### NFR-001: Response Time
**Requirement ID**: NFR-PERF-001  
**Title**: Thời gian phản hồi  
**Status**: PROPOSED

**Description**: Tất cả yêu cầu phải phản hồi trong < 3 giây.

**Measure**: P95 Response < 3P, P99 Response < 5P

**Acceptance Criteria**:
1. UC-PERF-001.1 - Response time P95 < 3P.
2. UC-PERF-001.2 - Response time P99 < 5P.
3. UC-PERF-001.3 - Search speed < 3P.
4. UC-PERF-001.4 - API latency < 200ms p95.

**Priority**: CRITICAL  
**Impact**: Systems - ảnh hưởng trực tiếp đến user experience.

---

#### NFR-002: Scalability
**Requirement ID**: NFR-PERF-002  
**Title**: Khả năng mở rộng  
**Status**: PROPOSED

**Description**: Hệ thống phải xử lý:
- 1000+ users đồng thời
- 1000+ downloads/ngày
- Tối thiểu 500 calls/phút

**Measure**: Load tests, Stress tests

**Acceptance Criteria**:
1. UC-SCAL-002.1 - Hỗ trợ 1000+ users đồng thời.
2. UC-SCAL-002.2 - Xử lý 1000+ downloads/ngày.
3. UC-SCAL-002.3 - API Calls/min > 500.
4. UC-SCAL-002.4 - Concurrent users > 200.

**Priority**: HIGH  
**Impact**: Systems - cần thiết cho bền vững.

---

### 2. Security

#### NFR-003: Authentication
**Requirement ID**: NFR-SEC-001  
**Title**: Xác thực an toàn  
**Status**: PROPOSED

**Description**: 
- Xác thực 2 bước (2FA)
- Session timeout < 30 phút
- Password.reset < 3 phút

**Measure**: 2FA support, Session timeout

**Acceptance Criteria**:
1. UC-SEC-003.1 - 2FA support.
2. UC-SEC-003.2 - Session timeout < 30 phút.
3. UC-SEC-003.3 - Password reset < 3 phút.
4. UC-SEC-003.4 - Token expiration/enhance.

**Priority**: HIGH  
**Impact**: Systems - ảnh hưởng đến trust.

---

#### NFR-004: Data Protection
**Requirement ID**: NFR-SEC-002  
**Title**: Bảo vệ dữ liệu  
**Status**: PROPOSED

**Description**: 
- Encryption at rest, in transit
- Regular backups
- Data retention management

**Measure**: Encryption, Backup frequency

**Acceptance Criteria**:
1. UC-SEC-004.1 - TLS 1.3 transport.
2. UC-SEC-004.2 - Encrypted storage.
3. UC-SEC-004.3 - Daily backups.
4. UC-SEC-004.4 - Backup restore < 24h.
5. UC-SEC-004.5 - Data retention policy implemented.

**Priority**: CRITICAL  
**Impact**: Trust - phải có.

---

### 3. Usability

#### NFR-005: User Experience
**Requirement ID**: NFR-USE-001  
**Title**: Trải nghiệm người dùng  
**Status**: PROPOSED

**Description**: 
- Giao diện tiếng Việt/Anh
- Response thời gian thực
- Hoạt động offline

**Measure**: UI coverage, Realtime updates, Offline support

**Acceptance Criteria**:
1. UC-USE-005.1 - 95% giao diện đã dịch SLVIEV.
2. UC-USE-005.2 - Realtime updates.
3. UC-USE-005.3 - Offline functionality supported.
4. UC-USE-005.4 - Mobile responsive design.

**Priority**: MEDIUM  
**Impact**: User experience - cải thiện.

---

## Requirements Summary

### Priority Matrix

| Priority | ID | Description | Impact |
|----------|----|-------------|--------|
| **CRITICAL** | NFR-PERF-001 | Response Time | Core Systems |
| **CRITICAL** | NFR-SEC-002 | Data Protection | Trust/Security |
| **HIGH** | NFR-PERF-002 | Scalability | Systems |
| **HIGH** | NFR-SEC-001 | Authentication | Trust/Security |
| **HIGH** | FR-004 | Search | Core feature |
| **HIGH** | FR-001 | PDF Upload | Core feature |
| **HIGH** | FR-002 | Tagging/Org | Core feature |
| **MEDIUM** | FR-003 | Auto-tagging | Enhancement |
| **MEDIUM** | NFR-USE-001 | UX | User experience |

---

### Key Dependencies

- **Storage** (FR-001, FR-002, NFR-004): MinIO
- **Search** (FR-004, NFR-001): Axiom
- **ML** (FR-003): AI service
- **Auth** (NFR-003): Keycloak
- **Database** (NFR-002): SQL Server

---

*See [Architectural Decisions](../Architecture decisions/Overview.md) for how requirements were translated into design decisions.*
