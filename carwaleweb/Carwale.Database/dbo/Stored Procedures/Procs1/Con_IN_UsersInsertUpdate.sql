IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_IN_UsersInsertUpdate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_IN_UsersInsertUpdate]
GO

	CREATE Procedure [dbo].[Con_IN_UsersInsertUpdate]
@ID NUMERIC,
@Name VARCHAR(100),
@Email VARCHAR(100),
@Mobile VARCHAR(20),
@Phone VARCHAR(20),
@ReceiveNewsletter BIT,
@CreateDate DATETIME,
@LastSaveId NUMERIC OUTPUT

AS
	
BEGIN
	IF @ID = -1
		BEGIN
		INSERT INTO Con_IN_Users 
		(Name,Email,Mobile,Phone,ReceiveNewsletter,CreateDate) 
		VALUES 
		(@Name,@Email,@Mobile,@Phone,@ReceiveNewsletter,@CreateDate)
		
		SET @LastSaveId = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN
		UPDATE Con_IN_Users
		SET Name=@Name , Email=@Email, Mobile=@Mobile,
		Phone = @Phone, ReceiveNewsletter=@ReceiveNewsletter
		WHERE ID = @ID
		SET @LastSaveId = @ID 
		END
END
