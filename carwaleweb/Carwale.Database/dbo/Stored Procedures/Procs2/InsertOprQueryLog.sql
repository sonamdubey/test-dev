IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertOprQueryLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertOprQueryLog]
GO
	-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <10/9/2014>
-- Description:	<Log Queries Fired From GoGreen>
-- =============================================
CREATE PROCEDURE [dbo].[InsertOprQueryLog]
@Query			VARCHAR(Max),
@QryFiredBy		INT,
@ID				INT OUTPUT
AS
BEGIN
	SET @ID = -1
	INSERT INTO Opr_QueryLog (Query , QryFiredBy)  VALUES(@Query , @QryFiredBy) 
	SET @ID = SCOPE_IDENTITY()

END
