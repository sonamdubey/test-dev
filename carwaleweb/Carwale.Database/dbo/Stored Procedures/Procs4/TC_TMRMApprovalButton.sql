IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TMRMApprovalButton]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TMRMApprovalButton]
GO

	-- =================================================================
-- Author	    :	Vinayk Patil
-- Create date	:	27-11-2013
-- Description	:	SP for Approval Button on Regional Manager screen 
-- Modified By	:	Nilesh Utture on 13-12-13, Added @Year parameter while updating approval master table 
-- Modified By  :   Nilesh Utture on 17th Dec, 2013 Added @Comments parameter
-- ==================================================================

CREATE PROCEDURE [dbo].[TC_TMRMApprovalButton]
@AprroversId INT,		------ ID of RM who approves or rejects revised targets
@TC_AMId INT,	------ ID of AM for whose targets are approved or rejected
@IsApproved BIT,    ------ 1 for approval & 0 for rejection
@Year INT, 
@Comments VARCHAR(1000) = NULL

AS 
 
 BEGIN

---------- If RM approves the target for particular AM then update TC_TMAMTargetChangeMaster-------
---------- for that AM if that record is active (i.e. IsActive = 1)--------------------------------

 IF( @IsApproved = 1 )
  BEGIN
      UPDATE TC_TMAMTargetChangeMaster
      SET    IsAprrovedByRM = 1,
             RMActionDate = GETDATE(),
             RMId = @AprroversId,
			 RMComments = @Comments -- Modified By  :   Nilesh Utture on 17th Dec, 2013
      WHERE  TC_AMId = @TC_AMId
			 AND [Year] = @Year -- Modified By	:	Nilesh Utture on 13-12-13
             AND IsActive = 1
  END
ELSE


---------- If RM rejects the target for particular AM then update TC_TMAMTargetChangeMaster for that-------
---------- AM if that record is active (i.e. IsActive = 1) & make IsActive = 0 for that record-------------

  BEGIN
      UPDATE TC_TMAMTargetChangeMaster
      SET    IsAprrovedByRM = 0,
             IsActive = 0,
             RMActionDate = GETDATE(),
             RMId = @AprroversId,
			 RMComments = @Comments -- Modified By  :   Nilesh Utture on 17th Dec, 2013
      WHERE  TC_AMId = @TC_AMId
			 AND [Year] = @Year -- Modified By	:	Nilesh Utture on 13-12-13
             AND IsActive = 1
  END  
END
