IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveUser]
GO

	
--Author : Vinay Kumar prajapati 12 desc 2014
-- Purpose : To Save ESM User.

CREATE PROCEDURE [dbo].[ESM_SaveUser]
(
@UserId  INT,
@Date DateTime,
@UpdatedBy INT,
@Status  BIT OUTPUT
)
AS
BEGIN

INSERT INTO ESM_users(UserId,CreatedOn, UpdatedOn, UpdatedBy, IsActive)
                  VALUES( @UserId, @Date,@Date, @UpdatedBy, 1)

				 SET @Status = 1
	
END

