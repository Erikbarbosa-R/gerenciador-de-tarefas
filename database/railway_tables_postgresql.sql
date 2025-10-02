-- Task Manager Database Schema para Railway
-- PostgreSQL - Railway

-- Tabela Users
CREATE TABLE IF NOT EXISTS Users (
    Id UUID NOT NULL PRIMARY KEY DEFAULT gen_random_uuid(),
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Role INTEGER NOT NULL DEFAULT 0,
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt TIMESTAMP(6) NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP(6) NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE
);

-- Índices para Users
CREATE INDEX IF NOT EXISTS IX_Users_Email ON Users(Email);
CREATE INDEX IF NOT EXISTS IX_Users_IsActive ON Users(IsActive);
CREATE INDEX IF NOT EXISTS IX_Users_CreatedAt ON Users(CreatedAt);

-- Tabela Tasks
CREATE TABLE IF NOT EXISTS Tasks (
    Id UUID NOT NULL PRIMARY KEY DEFAULT gen_random_uuid(),
    Title VARCHAR(200) NOT NULL,
    Description VARCHAR(1000) NOT NULL,
    Status INTEGER NOT NULL DEFAULT 0,
    Priority INTEGER NOT NULL DEFAULT 1,
    DueDate TIMESTAMP(6) NULL,
    UserId UUID NOT NULL,
    AssignedToUserId UUID NULL,
    CreatedAt TIMESTAMP(6) NOT NULL DEFAULT NOW(),
    UpdatedAt TIMESTAMP(6) NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE
);

-- Índices para Tasks
CREATE INDEX IF NOT EXISTS IX_Tasks_UserId ON Tasks(UserId);
CREATE INDEX IF NOT EXISTS IX_Tasks_AssignedToUserId ON Tasks(AssignedToUserId);
CREATE INDEX IF NOT EXISTS IX_Tasks_Status ON Tasks(Status);
CREATE INDEX IF NOT EXISTS IX_Tasks_Priority ON Tasks(Priority);
CREATE INDEX IF NOT EXISTS IX_Tasks_DueDate ON Tasks(DueDate);
CREATE INDEX IF NOT EXISTS IX_Tasks_CreatedAt ON Tasks(CreatedAt);

-- Adicionar foreign keys após criação das tabelas
ALTER TABLE Tasks ADD CONSTRAINT FK_Tasks_UserId FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE;
ALTER TABLE Tasks ADD CONSTRAINT FK_Tasks_AssignedToUserId FOREIGN KEY (AssignedToUserId) REFERENCES Users(Id) ON DELETE SET NULL;

-- Inserir dados de exemplo
INSERT INTO Users (Id, Name, Email, PasswordHash, Role, IsActive, CreatedAt, IsDeleted) VALUES
('550e8400-e29b-41d4-a716-446655440001', 'Admin Sistema', 'admin@exemplo.com', '$2a$11$exemplo.hash.aqui', 1, TRUE, NOW(), FALSE),
('550e8400-e29b-41d4-a716-446655440002', 'João Silva', 'joao@exemplo.com', '$2a$11$exemplo.hash.aqui', 0, TRUE, NOW(), FALSE),
('550e8400-e29b-41d4-a716-446655440003', 'Maria Santos', 'maria@exemplo.com', '$2a$11$exemplo.hash.aqui', 0, TRUE, NOW(), FALSE)
ON CONFLICT (Id) DO NOTHING;

INSERT INTO Tasks (Id, Title, Description, Status, Priority, DueDate, UserId, AssignedToUserId, CreatedAt, IsDeleted) VALUES
('660e8400-e29b-41d4-a716-446655440001', 'Implementar autenticação', 'Implementar sistema de autenticação JWT na API', 0, 2, NOW() + INTERVAL '7 days', '550e8400-e29b-41d4-a716-446655440001', '550e8400-e29b-41d4-a716-446655440002', NOW(), FALSE),
('660e8400-e29b-41d4-a716-446655440002', 'Criar documentação', 'Criar documentação da API com Swagger', 1, 1, NOW() + INTERVAL '3 days', '550e8400-e29b-41d4-a716-446655440002', '550e8400-e29b-41d4-a716-446655440003', NOW(), FALSE),
('660e8400-e29b-41d4-a716-446655440003', 'Testes unitários', 'Implementar testes unitários para todas as funcionalidades', 0, 3, NOW() + INTERVAL '14 days', '550e8400-e29b-41d4-a716-446655440003', NULL, NOW(), FALSE)
ON CONFLICT (Id) DO NOTHING;

-- Verificar dados inseridos
SELECT 'Users' as Tabela, COUNT(*) as Total FROM Users WHERE IsDeleted = FALSE
UNION ALL
SELECT 'Tasks' as Tabela, COUNT(*) as Total FROM Tasks WHERE IsDeleted = FALSE;
