-- Task Manager Database Schema
-- MySQL 8.0+ - TaskManagerDb

-- Criar database TaskManagerDb
CREATE DATABASE IF NOT EXISTS TaskManagerDb;

-- Usar o database TaskManagerDb
USE TaskManagerDb;

-- Verificar se estamos no banco correto
SELECT 'Usando database: ' + DATABASE() as Status;

-- Tabela Users
CREATE TABLE IF NOT EXISTS Users (
    Id CHAR(36) NOT NULL PRIMARY KEY DEFAULT (UUID()),
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(255) NOT NULL UNIQUE,
    PasswordHash VARCHAR(255) NOT NULL,
    Role INT NOT NULL DEFAULT 0,
    IsActive BOOLEAN NOT NULL DEFAULT TRUE,
    CreatedAt DATETIME(6) NOT NULL,
    UpdatedAt DATETIME(6) NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE
);

-- Criar índices para Users
CREATE INDEX IX_Users_IsActive ON Users (IsActive);
CREATE INDEX IX_Users_CreatedAt ON Users (CreatedAt);
CREATE INDEX IX_Users_Email ON Users (Email);
CREATE INDEX IX_Users_Role ON Users (Role);
CREATE INDEX IX_Users_IsDeleted ON Users (IsDeleted);

-- Tabela Tasks
CREATE TABLE IF NOT EXISTS Tasks (
    Id CHAR(36) NOT NULL PRIMARY KEY DEFAULT (UUID()),
    Title VARCHAR(200) NOT NULL,
    Description VARCHAR(1000) NOT NULL,
    Status INT NOT NULL DEFAULT 0,
    Priority INT NOT NULL DEFAULT 1,
    DueDate DATETIME(6) NULL,
    UserId CHAR(36) NOT NULL,
    CreatedAt DATETIME(6) NOT NULL,
    UpdatedAt DATETIME(6) NULL,
    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE
);

-- Criar índices para Tasks
CREATE INDEX IX_Tasks_UserId ON Tasks (UserId);
CREATE INDEX IX_Tasks_Status ON Tasks (Status);
CREATE INDEX IX_Tasks_Priority ON Tasks (Priority);
CREATE INDEX IX_Tasks_DueDate ON Tasks (DueDate);
CREATE INDEX IX_Tasks_CreatedAt ON Tasks (CreatedAt);
CREATE INDEX IX_Tasks_IsDeleted ON Tasks (IsDeleted);
CREATE INDEX IX_Tasks_Title ON Tasks (Title);
CREATE INDEX IX_Tasks_Status_Priority ON Tasks (Status, Priority);
CREATE INDEX IX_Tasks_UserId_Status ON Tasks (UserId, Status);
CREATE INDEX IX_Tasks_DueDate_Status ON Tasks (DueDate, Status);

-- Inserir dados de exemplo
INSERT IGNORE INTO Users (Id, Name, Email, PasswordHash, Role, IsActive, CreatedAt, IsDeleted) VALUES
('550e8400-e29b-41d4-a716-446655440001', 'Admin Sistema', 'admin@exemplo.com', '$2a$11$exemplo.hash.aqui', 1, TRUE, NOW(), FALSE),
('550e8400-e29b-41d4-a716-446655440002', 'João Silva', 'joao@exemplo.com', '$2a$11$exemplo.hash.aqui', 0, TRUE, NOW(), FALSE),
('550e8400-e29b-41d4-a716-446655440003', 'Maria Santos', 'maria@exemplo.com', '$2a$11$exemplo.hash.aqui', 0, TRUE, NOW(), FALSE);

INSERT IGNORE INTO Tasks (Id, Title, Description, Status, Priority, DueDate, UserId, CreatedAt, IsDeleted) VALUES
('660e8400-e29b-41d4-a716-446655440001', 'Implementar autenticação', 'Implementar sistema de autenticação JWT na API', 0, 2, DATE_ADD(NOW(), INTERVAL 7 DAY), '550e8400-e29b-41d4-a716-446655440001', NOW(), FALSE),
('660e8400-e29b-41d4-a716-446655440002', 'Criar documentação', 'Criar documentação da API com Swagger', 1, 1, DATE_ADD(NOW(), INTERVAL 3 DAY), '550e8400-e29b-41d4-a716-446655440002', NOW(), FALSE),
('660e8400-e29b-41d4-a716-446655440003', 'Testes unitários', 'Implementar testes unitários para todas as funcionalidades', 0, 3, DATE_ADD(NOW(), INTERVAL 14 DAY), '550e8400-e29b-41d4-a716-446655440003', NOW(), FALSE);

-- Adicionar foreign key após criação das tabelas
ALTER TABLE Tasks ADD FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE;

-- Verificar dados inseridos
SELECT 'Users' as Tabela, COUNT(*) as Total FROM Users WHERE IsDeleted = FALSE
UNION ALL
SELECT 'Tasks' as Tabela, COUNT(*) as Total FROM Tasks WHERE IsDeleted = FALSE;

-- Verificar índices criados
SELECT 
    TABLE_NAME as Tabela,
    INDEX_NAME as Indice,
    COLUMN_NAME as Coluna
FROM information_schema.STATISTICS 
WHERE table_schema = 'TaskManagerDb' 
AND table_name IN ('Users', 'Tasks')
ORDER BY TABLE_NAME, INDEX_NAME;

-- Verificar estrutura final
SELECT 'Script executado com sucesso no TaskManagerDb!' as Status;
