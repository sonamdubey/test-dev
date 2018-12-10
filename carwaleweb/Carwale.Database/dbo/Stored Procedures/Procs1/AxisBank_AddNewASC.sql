IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_AddNewASC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_AddNewASC]
GO

	
-- =============================================
-- Author:		Akansha
-- Create date: 20.12.2013
-- Description:	Add New ASC
-- =============================================
Create PROCEDURE AxisBank_AddNewASC 
@Name VARCHAR(100),
@Inserted BIT OUTPUT
AS
BEGIN
	SET @Inserted = 0

	SELECT ID
	FROM AxisBank_ASC
	WHERE NAME = @Name

	IF @@ROWCOUNT < 1
	BEGIN
		INSERT INTO AxisBank_ASC (NAME)
		VALUES (@Name)
		SET @Inserted =1
	END

	Select @Inserted
END

