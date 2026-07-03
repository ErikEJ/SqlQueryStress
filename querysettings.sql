-- Default SQL Server Management Studio query execution settings
-- These settings match the default SSMS configuration for query execution
-- ANSI settings
SET QUOTED_IDENTIFIER ON;
SET ANSI_NULL_DFLT_ON ON;
SET ANSI_PADDING ON;
SET ANSI_WARNINGS ON;
SET ANSI_NULLS ON;
-- Advanced settings
SET ARITHABORT ON;
SET CONCAT_NULL_YIELDS_NULL ON;
-- Additional settings (default OFF in SSMS)
SET IMPLICIT_TRANSACTIONS OFF;
SET CURSOR_CLOSE_ON_COMMIT OFF;
BEGIN TRY
    EXEC('SET XACT_ABORT OFF');
END TRY
BEGIN CATCH
    PRINT 'Cannot set XACT_ABORT OFF: ' + ERROR_MESSAGE();
END CATCH;
