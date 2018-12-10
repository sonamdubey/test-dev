IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquirySourceSelect]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquirySourceSelect]
GO

	-- Author:		Surendra
-- Create date: 12 Jan 2012
-- Description:	This procedure is used to get Inquiry Source
-- Modified By : Tejashree Patil on 4 Feb 2013 at 11 am; Added condition to display Inquiry Source in dropdown.
-- Modified By: Nilesh Utture on 12th September, 2013 Added MakeId in Select Field
--Modified By: Afrose on 7th July 2015, changed where clause to Isvisible CW and BW for new group sources
--Modified By Vivek Gupta on 17-07-2015 , added @Branchid to getr sources from BW and CW
-- =============================================
CREATE PROCEDURE [dbo].[TC_InquirySourceSelect]
@IsFilterDropdown SMALLINT = NULL,
@BranchId INT = NULL
AS
BEGIN	

DECLARE @ApplicationId TINYINT = 1

IF @BranchId IS NOT NULL
 SET @ApplicationId = (SELECT ApplicationId FROM Dealers WITH(NOLOCK) WHERE Id = @BranchId)

	IF @IsFilterDropdown IS NULL
	BEGIN
	  IF(@ApplicationId = 1)
	  BEGIN
		SELECT	Id, Source, MakeId 
		FROM	TC_InquirySource 
		WHERE	IsActive=1 AND IsVisibleCW = 1
	  END
	  ELSE IF (@ApplicationId = 2)
	  BEGIN
	     SELECT	Id, Source, MakeId 
		FROM	TC_InquirySource 
		WHERE	IsActive=1 AND IsVisibleBW = 1
	  END
	END
	ELSE
	BEGIN	
		SELECT	Id, Source 
		FROM	TC_InquirySource 
		WHERE	IsActive=1 
	END
END








/****** Object:  StoredProcedure [dbo].[TC_FetchInqCountForReports]    Script Date: 15 07 2015 17:32:52 ******/
SET ANSI_NULLS ON
