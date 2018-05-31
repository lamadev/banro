
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/07/2017 12:34:59
-- Generated from EDMX file: C:\Users\user\Desktop\BanroWebApp\BanroWebApp\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [BANRO];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_employee_contractor_t_contractor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[employee_contractor] DROP CONSTRAINT [FK_employee_contractor_t_contractor];
GO
IF OBJECT_ID(N'[dbo].[FK_t_beneficiaires_t_succursales]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[t_beneficiaires] DROP CONSTRAINT [FK_t_beneficiaires_t_succursales];
GO
IF OBJECT_ID(N'[dbo].[FK_t_bon_commandes_t_centre_soins]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[t_bon_commandes] DROP CONSTRAINT [FK_t_bon_commandes_t_centre_soins];
GO
IF OBJECT_ID(N'[dbo].[FK_t_contractor_t_succursales]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[t_contractor] DROP CONSTRAINT [FK_t_contractor_t_succursales];
GO
IF OBJECT_ID(N'[dbo].[FK_t_departement_t_succursales]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[t_departement] DROP CONSTRAINT [FK_t_departement_t_succursales];
GO
IF OBJECT_ID(N'[dbo].[FK_t_factures_t_bon_commandes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[t_factures] DROP CONSTRAINT [FK_t_factures_t_bon_commandes];
GO
IF OBJECT_ID(N'[dbo].[FK_t_vouchers_casuals_t_centre_soins]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[t_vouchers_casuals] DROP CONSTRAINT [FK_t_vouchers_casuals_t_centre_soins];
GO
IF OBJECT_ID(N'[dbo].[FK_t_vouchers_casuals_t_succursales]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[t_vouchers_casuals] DROP CONSTRAINT [FK_t_vouchers_casuals_t_succursales];
GO
IF OBJECT_ID(N'[dbo].[FK_t_vouchers_contractor_employee_contractor]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[t_vouchers_contractor] DROP CONSTRAINT [FK_t_vouchers_contractor_employee_contractor];
GO
IF OBJECT_ID(N'[dbo].[FK_t_vouchers_contractor_t_centre_soins]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[t_vouchers_contractor] DROP CONSTRAINT [FK_t_vouchers_contractor_t_centre_soins];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[employee_contractor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[employee_contractor];
GO
IF OBJECT_ID(N'[dbo].[sysdiagrams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sysdiagrams];
GO
IF OBJECT_ID(N'[dbo].[t_beneficiaires]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_beneficiaires];
GO
IF OBJECT_ID(N'[dbo].[t_bon_commandes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_bon_commandes];
GO
IF OBJECT_ID(N'[dbo].[t_categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_categories];
GO
IF OBJECT_ID(N'[dbo].[t_centre_soins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_centre_soins];
GO
IF OBJECT_ID(N'[dbo].[t_contractor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_contractor];
GO
IF OBJECT_ID(N'[dbo].[t_departement]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_departement];
GO
IF OBJECT_ID(N'[dbo].[t_factures]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_factures];
GO
IF OBJECT_ID(N'[dbo].[t_logger]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_logger];
GO
IF OBJECT_ID(N'[dbo].[t_sickness]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_sickness];
GO
IF OBJECT_ID(N'[dbo].[t_succursales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_succursales];
GO
IF OBJECT_ID(N'[dbo].[t_test]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_test];
GO
IF OBJECT_ID(N'[dbo].[t_vouchers_casuals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_vouchers_casuals];
GO
IF OBJECT_ID(N'[dbo].[t_vouchers_contractor]', 'U') IS NOT NULL
    DROP TABLE [dbo].[t_vouchers_contractor];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'sysdiagrams'
CREATE TABLE [dbo].[sysdiagrams] (
    [name] nvarchar(128)  NOT NULL,
    [principal_id] int  NOT NULL,
    [diagram_id] int IDENTITY(1,1) NOT NULL,
    [version] int  NULL,
    [definition] varbinary(max)  NULL
);
GO

-- Creating table 't_bon_commandes'
CREATE TABLE [dbo].[t_bon_commandes] (
    [C_id_bon] int IDENTITY(1,1) NOT NULL,
    [C_id_bene] int  NULL,
    [C_id_centre] int  NULL,
    [C_datedeb] varchar(50)  NULL,
    [C_datefin] varchar(50)  NULL,
    [C_namedoctor] varchar(50)  NULL,
    [C_approuve] varchar(50)  NULL,
    [C_motif] varchar(50)  NULL
);
GO

-- Creating table 't_categories'
CREATE TABLE [dbo].[t_categories] (
    [C_id_categ] varchar(50)  NOT NULL
);
GO

-- Creating table 't_centre_soins'
CREATE TABLE [dbo].[t_centre_soins] (
    [C_id_centre] int IDENTITY(1,1) NOT NULL,
    [C_name] varchar(50)  NULL,
    [adresse] varchar(50)  NULL,
    [C_phone] varchar(50)  NULL
);
GO

-- Creating table 't_departement'
CREATE TABLE [dbo].[t_departement] (
    [C_id] int IDENTITY(1,1) NOT NULL,
    [C_id_depart] varchar(50)  NOT NULL,
    [C_id_succ] varchar(50)  NULL
);
GO

-- Creating table 't_factures'
CREATE TABLE [dbo].[t_factures] (
    [C_id_fact] int IDENTITY(1,1) NOT NULL,
    [C_id_bon] int  NULL,
    [C_datefacture] varchar(50)  NULL,
    [C_timefacture] varchar(50)  NULL,
    [C_cout] decimal(19,4)  NULL,
    [C_fileupload] varchar(150)  NULL
);
GO

-- Creating table 't_succursales'
CREATE TABLE [dbo].[t_succursales] (
    [C_id] varchar(50)  NOT NULL,
    [C_name] varchar(100)  NULL,
    [C_adresse] nvarchar(50)  NULL,
    [C_phone] varchar(50)  NULL
);
GO

-- Creating table 't_test'
CREATE TABLE [dbo].[t_test] (
    [id] int IDENTITY(1,1) NOT NULL,
    [name] varchar(50)  NULL
);
GO

-- Creating table 't_contractor'
CREATE TABLE [dbo].[t_contractor] (
    [C_id] int IDENTITY(1,1) NOT NULL,
    [C_name] varchar(50)  NULL,
    [C_adresse] varchar(50)  NULL,
    [C_phone] varchar(50)  NULL,
    [C_idSucc] varchar(50)  NULL
);
GO

-- Creating table 'employee_contractor'
CREATE TABLE [dbo].[employee_contractor] (
    [C_id] int IDENTITY(1,1) NOT NULL,
    [C_name] varchar(50)  NULL,
    [C_sex] varchar(50)  NULL,
    [C_adresse] varchar(50)  NULL,
    [C_phone] varchar(50)  NULL,
    [C_idContractor] int  NULL
);
GO

-- Creating table 't_beneficiaires'
CREATE TABLE [dbo].[t_beneficiaires] (
    [C_id] int IDENTITY(1,1) NOT NULL,
    [C_id_succ] varchar(50)  NULL,
    [C_name] varchar(50)  NULL,
    [C_datenais] varchar(50)  NULL,
    [C_sex] varchar(1)  NULL,
    [C_statusmaritalk] varchar(50)  NULL,
    [C_phone] varchar(15)  NULL,
    [C_picture] varchar(max)  NULL,
    [C_id_partenaire] varchar(50)  NULL,
    [C_id_parent] varchar(50)  NULL,
    [C_id_categorie] varchar(50)  NULL,
    [C_id_depart] int  NULL,
    [C_statusChild] varchar(50)  NULL,
    [C_id_visitor] varchar(50)  NULL,
    [C_motif_visit] varchar(max)  NULL
);
GO

-- Creating table 't_sickness'
CREATE TABLE [dbo].[t_sickness] (
    [C_Name] varchar(50)  NOT NULL
);
GO

-- Creating table 't_vouchers_casuals'
CREATE TABLE [dbo].[t_vouchers_casuals] (
    [C_id_voucher] int IDENTITY(1,1) NOT NULL,
    [C_name_casual] varchar(50)  NULL,
    [C_company_casual] varchar(50)  NULL,
    [C_date_casual] varchar(50)  NULL,
    [C_motif] varchar(50)  NULL,
    [C_id_centre] int  NULL,
    [C_id_company] varchar(50)  NULL,
    [C_cause] varchar(150)  NULL,
    [C_cout] decimal(19,4)  NULL
);
GO

-- Creating table 't_vouchers_contractor'
CREATE TABLE [dbo].[t_vouchers_contractor] (
    [C_id_voucher] int IDENTITY(1,1) NOT NULL,
    [C_id_Employed] int  NULL,
    [C_id_centre] int  NULL,
    [C_datedeb] varchar(50)  NULL,
    [C_datefin] varchar(50)  NULL,
    [C_namedoctor] varchar(50)  NULL,
    [C_approuve] varchar(50)  NULL,
    [C_motif] varchar(50)  NULL,
    [C_cout] decimal(19,4)  NULL
);
GO

-- Creating table 't_logger'
CREATE TABLE [dbo].[t_logger] (
    [C_id] int IDENTITY(1,1) NOT NULL,
    [C_username] varchar(50)  NULL,
    [password] nvarchar(max)  NULL,
    [C_priority] varchar(50)  NULL,
    [C_idSucc] varchar(50)  NULL,
    [C_employeeId] int  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [diagram_id] in table 'sysdiagrams'
ALTER TABLE [dbo].[sysdiagrams]
ADD CONSTRAINT [PK_sysdiagrams]
    PRIMARY KEY CLUSTERED ([diagram_id] ASC);
GO

-- Creating primary key on [C_id_bon] in table 't_bon_commandes'
ALTER TABLE [dbo].[t_bon_commandes]
ADD CONSTRAINT [PK_t_bon_commandes]
    PRIMARY KEY CLUSTERED ([C_id_bon] ASC);
GO

-- Creating primary key on [C_id_categ] in table 't_categories'
ALTER TABLE [dbo].[t_categories]
ADD CONSTRAINT [PK_t_categories]
    PRIMARY KEY CLUSTERED ([C_id_categ] ASC);
GO

-- Creating primary key on [C_id_centre] in table 't_centre_soins'
ALTER TABLE [dbo].[t_centre_soins]
ADD CONSTRAINT [PK_t_centre_soins]
    PRIMARY KEY CLUSTERED ([C_id_centre] ASC);
GO

-- Creating primary key on [C_id] in table 't_departement'
ALTER TABLE [dbo].[t_departement]
ADD CONSTRAINT [PK_t_departement]
    PRIMARY KEY CLUSTERED ([C_id] ASC);
GO

-- Creating primary key on [C_id_fact] in table 't_factures'
ALTER TABLE [dbo].[t_factures]
ADD CONSTRAINT [PK_t_factures]
    PRIMARY KEY CLUSTERED ([C_id_fact] ASC);
GO

-- Creating primary key on [C_id] in table 't_succursales'
ALTER TABLE [dbo].[t_succursales]
ADD CONSTRAINT [PK_t_succursales]
    PRIMARY KEY CLUSTERED ([C_id] ASC);
GO

-- Creating primary key on [id] in table 't_test'
ALTER TABLE [dbo].[t_test]
ADD CONSTRAINT [PK_t_test]
    PRIMARY KEY CLUSTERED ([id] ASC);
GO

-- Creating primary key on [C_id] in table 't_contractor'
ALTER TABLE [dbo].[t_contractor]
ADD CONSTRAINT [PK_t_contractor]
    PRIMARY KEY CLUSTERED ([C_id] ASC);
GO

-- Creating primary key on [C_id] in table 'employee_contractor'
ALTER TABLE [dbo].[employee_contractor]
ADD CONSTRAINT [PK_employee_contractor]
    PRIMARY KEY CLUSTERED ([C_id] ASC);
GO

-- Creating primary key on [C_id] in table 't_beneficiaires'
ALTER TABLE [dbo].[t_beneficiaires]
ADD CONSTRAINT [PK_t_beneficiaires]
    PRIMARY KEY CLUSTERED ([C_id] ASC);
GO

-- Creating primary key on [C_Name] in table 't_sickness'
ALTER TABLE [dbo].[t_sickness]
ADD CONSTRAINT [PK_t_sickness]
    PRIMARY KEY CLUSTERED ([C_Name] ASC);
GO

-- Creating primary key on [C_id_voucher] in table 't_vouchers_casuals'
ALTER TABLE [dbo].[t_vouchers_casuals]
ADD CONSTRAINT [PK_t_vouchers_casuals]
    PRIMARY KEY CLUSTERED ([C_id_voucher] ASC);
GO

-- Creating primary key on [C_id_voucher] in table 't_vouchers_contractor'
ALTER TABLE [dbo].[t_vouchers_contractor]
ADD CONSTRAINT [PK_t_vouchers_contractor]
    PRIMARY KEY CLUSTERED ([C_id_voucher] ASC);
GO

-- Creating primary key on [C_id] in table 't_logger'
ALTER TABLE [dbo].[t_logger]
ADD CONSTRAINT [PK_t_logger]
    PRIMARY KEY CLUSTERED ([C_id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [C_id_centre] in table 't_bon_commandes'
ALTER TABLE [dbo].[t_bon_commandes]
ADD CONSTRAINT [FK_t_bon_commandes_t_centre_soins]
    FOREIGN KEY ([C_id_centre])
    REFERENCES [dbo].[t_centre_soins]
        ([C_id_centre])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_t_bon_commandes_t_centre_soins'
CREATE INDEX [IX_FK_t_bon_commandes_t_centre_soins]
ON [dbo].[t_bon_commandes]
    ([C_id_centre]);
GO

-- Creating foreign key on [C_id_bon] in table 't_factures'
ALTER TABLE [dbo].[t_factures]
ADD CONSTRAINT [FK_t_factures_t_bon_commandes]
    FOREIGN KEY ([C_id_bon])
    REFERENCES [dbo].[t_bon_commandes]
        ([C_id_bon])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_t_factures_t_bon_commandes'
CREATE INDEX [IX_FK_t_factures_t_bon_commandes]
ON [dbo].[t_factures]
    ([C_id_bon]);
GO

-- Creating foreign key on [C_id_succ] in table 't_departement'
ALTER TABLE [dbo].[t_departement]
ADD CONSTRAINT [FK_t_departement_t_succursales]
    FOREIGN KEY ([C_id_succ])
    REFERENCES [dbo].[t_succursales]
        ([C_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_t_departement_t_succursales'
CREATE INDEX [IX_FK_t_departement_t_succursales]
ON [dbo].[t_departement]
    ([C_id_succ]);
GO

-- Creating foreign key on [C_idSucc] in table 't_contractor'
ALTER TABLE [dbo].[t_contractor]
ADD CONSTRAINT [FK_t_contractor_t_succursales]
    FOREIGN KEY ([C_idSucc])
    REFERENCES [dbo].[t_succursales]
        ([C_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_t_contractor_t_succursales'
CREATE INDEX [IX_FK_t_contractor_t_succursales]
ON [dbo].[t_contractor]
    ([C_idSucc]);
GO

-- Creating foreign key on [C_idContractor] in table 'employee_contractor'
ALTER TABLE [dbo].[employee_contractor]
ADD CONSTRAINT [FK_employee_contractor_t_contractor]
    FOREIGN KEY ([C_idContractor])
    REFERENCES [dbo].[t_contractor]
        ([C_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_employee_contractor_t_contractor'
CREATE INDEX [IX_FK_employee_contractor_t_contractor]
ON [dbo].[employee_contractor]
    ([C_idContractor]);
GO

-- Creating foreign key on [C_id_succ] in table 't_beneficiaires'
ALTER TABLE [dbo].[t_beneficiaires]
ADD CONSTRAINT [FK_t_beneficiaires_t_succursales]
    FOREIGN KEY ([C_id_succ])
    REFERENCES [dbo].[t_succursales]
        ([C_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_t_beneficiaires_t_succursales'
CREATE INDEX [IX_FK_t_beneficiaires_t_succursales]
ON [dbo].[t_beneficiaires]
    ([C_id_succ]);
GO

-- Creating foreign key on [C_id_centre] in table 't_vouchers_casuals'
ALTER TABLE [dbo].[t_vouchers_casuals]
ADD CONSTRAINT [FK_t_vouchers_casuals_t_centre_soins]
    FOREIGN KEY ([C_id_centre])
    REFERENCES [dbo].[t_centre_soins]
        ([C_id_centre])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_t_vouchers_casuals_t_centre_soins'
CREATE INDEX [IX_FK_t_vouchers_casuals_t_centre_soins]
ON [dbo].[t_vouchers_casuals]
    ([C_id_centre]);
GO

-- Creating foreign key on [C_id_company] in table 't_vouchers_casuals'
ALTER TABLE [dbo].[t_vouchers_casuals]
ADD CONSTRAINT [FK_t_vouchers_casuals_t_succursales]
    FOREIGN KEY ([C_id_company])
    REFERENCES [dbo].[t_succursales]
        ([C_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_t_vouchers_casuals_t_succursales'
CREATE INDEX [IX_FK_t_vouchers_casuals_t_succursales]
ON [dbo].[t_vouchers_casuals]
    ([C_id_company]);
GO

-- Creating foreign key on [C_id_Employed] in table 't_vouchers_contractor'
ALTER TABLE [dbo].[t_vouchers_contractor]
ADD CONSTRAINT [FK_t_vouchers_contractor_employee_contractor]
    FOREIGN KEY ([C_id_Employed])
    REFERENCES [dbo].[employee_contractor]
        ([C_id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_t_vouchers_contractor_employee_contractor'
CREATE INDEX [IX_FK_t_vouchers_contractor_employee_contractor]
ON [dbo].[t_vouchers_contractor]
    ([C_id_Employed]);
GO

-- Creating foreign key on [C_id_centre] in table 't_vouchers_contractor'
ALTER TABLE [dbo].[t_vouchers_contractor]
ADD CONSTRAINT [FK_t_vouchers_contractor_t_centre_soins]
    FOREIGN KEY ([C_id_centre])
    REFERENCES [dbo].[t_centre_soins]
        ([C_id_centre])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_t_vouchers_contractor_t_centre_soins'
CREATE INDEX [IX_FK_t_vouchers_contractor_t_centre_soins]
ON [dbo].[t_vouchers_contractor]
    ([C_id_centre]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------