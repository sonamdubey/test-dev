IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateUserImageUrl]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateUserImageUrl]
GO

	-- =============================================
-- Author:		<Author,Upendra Kumar>
-- Create date: <Create Date,28th December, 2015>
-- Description : To update User Image Url 
--==========================================
CREATE PROCEDURE [dbo].[TC_UpdateUserImageUrl]
(
 @type INT,				-- 1 -> url updation , 2-> IsShownOnCarWale updation 
 @UserId INT,
 @isShownOnCarwale BIT = NULL,
 @Url VARCHAR(100) = NULL
 )
 AS
BEGIN
  SET NOCOUNT ON;
  IF(@type = 1)
    BEGIN 
		UPDATE TC_Users 
		SET ImageUrl = @Url WHERE Id = @UserId
    END
 ELSE IF(@type = 2)
   BEGIN 
       DECLARE @BranchId INT
	   SELECT @BranchId = BranchId FROM TC_Users WITH(NOLOCK) WHERE Id = @UserId

	   UPDATE  TC_Users   SET IsShownCarwale = 0 WHERE BranchId = @BranchId

      UPDATE TC_Users SET IsShownCarwale = @isShownOnCarwale WHERE Id = @UserId
   END
END

