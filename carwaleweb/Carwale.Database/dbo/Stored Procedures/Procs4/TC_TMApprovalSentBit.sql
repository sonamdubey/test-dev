IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMApprovalSentBit]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMApprovalSentBit]
GO

	-- =============================================
-- Author	    :	Vinayak Patil
-- Create date	:	29-11-2013
-- Description	:	Returns whether updated revised targets are sent for approval .
-- =============================================
 CREATE PROCEDURE [dbo].[TC_TMApprovalSentBit]
 @TC_AMId INT,
 @IsSentForApproval BIT =0 OUTPUT

 AS
  
  BEGIN

  SELECT TOP 1 @IsSentForApproval = 1 FROM TC_TMAMTargetChangeMaster WITH(NOLOCK)
  WHERE TC_AMId = @TC_AMId
  AND IsActive = 1

  RETURN @IsSentForApproval



  END
