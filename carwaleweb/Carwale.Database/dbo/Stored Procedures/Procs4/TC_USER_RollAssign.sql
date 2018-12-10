IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_USER_RollAssign]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_USER_RollAssign]
GO

	
-- =============================================
-- ModifiedBy:		Binumon George
-- Create date: 08-11-2011
-- Description:	Added ModifiedBy parameter and modified date.
-- =============================================
-- =============================================
-- Author:		Binumon George
-- Create date: 13-06-2011
-- Description:	Assign Role to the user.
-- =============================================
CREATE procedure [dbo].[TC_USER_RollAssign]
(
@RollId INT,
@UserID INT,
@ModifiedBy INT
)
AS
UPDATE TC_Users SET RoleId=@RollId, ModifiedBy=@ModifiedBy,ModifiedDate=GETDATE()WHERE Id=@UserID 


/****** Object:  StoredProcedure [dbo].[TC_Users_SP]    Script Date: 11/10/2011 18:08:45 ******/
SET ANSI_NULLS ON
