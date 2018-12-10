IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DispositionLogInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DispositionLogInsert]
GO
	

-- =============================================  
-- Author:  Manish  
-- Create date: 29-Jan-13
-- Details: Inserting record in Disposition log for every disposition.
-- Modified By : Tejashree Patil on 13 Jul 2013, Added @EventCreatedOn to insert actual date of td given in imported excel.
-- Modified By : Khushaboo Patil on 27/07/2015 added @DispositionReason 
-- Modified By Vivek Gupta , on 09-01-2016 , added subdispositionID
-- =============================================  
CREATE PROCEDURE [dbo].[TC_DispositionLogInsert]  @TC_Usersid           AS  INT,  --User id of the event
                                                 @TC_LeadDispositionId AS TINYINT, ---Disposition id of the event
                                                 @ItemId               AS INT,   --Id of the table where event executed
                                                 @TC_DispositionItemId AS TINYINT, -- Id of the Item on which event executed
                                                 @TC_LeadId            AS INT,  ----Id of the lead
                                                 @EventCreatedOn	   AS DATETIME = NULL,
												 @DispositionReason	   AS VARCHAR(200)= NULL,
												 @SubDispositionId     AS INT = NULL
AS 
     BEGIN 
           
			SET @EventCreatedOn = ISNULL(@EventCreatedOn,GETDATE())
           
           ----------Insert Record in TC_DispositionLog table--------
             INSERT INTO TC_DispositionLog
							                (TC_LeadDispositionId,
										     InqOrLeadId,
											 TC_DispositionItemId,
											 EventOwnerId,
											 TC_LeadId,
											 EventCreatedOn,
											 DispositionReason,
											 TC_SubDispositionId)
							  VALUES   (@TC_LeadDispositionId,
										@ItemId,
										@TC_DispositionItemId,
										@TC_Usersid,
										@TC_LeadId,
										@EventCreatedOn,
										@DispositionReason,
										@SubDispositionId
									   )
    END 













/****** Object:  StoredProcedure [dbo].[TC_GetModelOnBranchId]    Script Date: 1/13/2016 12:00:55 PM ******/
SET ANSI_NULLS ON
