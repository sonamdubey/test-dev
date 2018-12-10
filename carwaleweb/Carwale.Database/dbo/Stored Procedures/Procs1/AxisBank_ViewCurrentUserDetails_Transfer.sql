IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_ViewCurrentUserDetails_Transfer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_ViewCurrentUserDetails_Transfer]
GO

	


-- =============================================
-- Author:		Kumar Vikram
-- Create date: 18.12.2013
-- Description:	Gets the existing current User details
-- exec AxisBank_ViewCurrentUserDetails  
-- =============================================
 CREATE PROCEDURE [dbo].[AxisBank_ViewCurrentUserDetails_Transfer]
	(
	@SearchUser VARCHAR(50) = NULL
	)
	AS
	
BEGIN 

SET NOCOUNT ON
	IF (@SearchUser is null OR  @SearchUser='')
			
		BEGIN
			SELECT UserId,
				   LoginId,
				   FirstName,
				   LastName,
				   CreatedOn,
				   IsActive,
				   IsAdmin,
				   Email
			FROM AxisBank_Users ORDER BY CreatedOn DESC

			SELECT UserId,
				   LoginId,
				   FirstName,
				   LastName,
				   CreatedOn,
				   IsActive,
				   IsAdmin,
				   Email
			FROM AxisBank_Users WHERE IsActive = 1 ORDER BY CreatedOn DESC
		END
	ELSE
		BEGIN
		SELECT Top 1 UserId
			FROM AxisBank_Users WHERE FirstName LIKE  @SearchUser OR LastName LIKE  @SearchUser OR LoginId LIKE  @SearchUser  ORDER BY CreatedOn DESC

		IF @@ROWCOUNT <> 0
		BEGIN
			SELECT UserId,
					   LoginId,
					   FirstName,
					   LastName,
					   CreatedOn,
					   IsActive,
					   IsAdmin,
					   Email
				FROM AxisBank_Users WHERE FirstName LIKE  @SearchUser OR LastName LIKE  @SearchUser OR LoginId LIKE  @SearchUser  ORDER BY CreatedOn DESC
			END
			ELSE
			BEGIN
					SELECT UserId,
							LoginId,
							FirstName,
							LastName,
							CreatedOn,
							IsActive,
							IsAdmin,
							Email
					FROM AxisBank_Users ORDER BY CreatedOn DESC
					END
		END
 END


