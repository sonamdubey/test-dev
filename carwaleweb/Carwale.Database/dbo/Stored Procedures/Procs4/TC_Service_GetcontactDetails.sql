IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Service_GetcontactDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Service_GetcontactDetails]
GO

	-- =============================================
-- Author:		Nilima More 
-- Create date: 24 Oct 2016
-- Description:	To get serice contact details.
--EXEC TC_Service_GetcontactDetails 20466,null,NULL,'MHO'
-- =============================================
CREATE PROCEDURE  [dbo].[TC_Service_GetcontactDetails] 
	 @BranchId INT 
	,@CustomerName VARCHAR(50) = NULL
	,@MobileNumber VARCHAR(50) = NULL
	,@CarRegistrationNumber VARCHAR(50) = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT CD.CustomerName,CD.Mobile,VW.Car,TSR.RegistrationNumber,TSR.ServiceDueDate,IL.TC_InquiriesLeadId,CD.Id CustomerId
		 ,RowNumber=ROW_NUMBER() OVER 
		( ORDER BY  TSR.ServiceDueDate ASC)
	FROM TC_Service_Reminder TSR WITH(NOLOCK)
	INNER JOIN vwMMV VW WITH (NOLOCK) ON VW.VersionId = TSR.VersionId
	INNER JOIN TC_CustomerDetails CD WITH (NOLOCK) ON TSR.CustomerId = CD.ID
	INNER JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_CustomerId = cd.Id
	WHERE TSR.BranchId  =@BranchId AND 	(@CustomerName IS NULL
			OR CD.CustomerName LIKE '%' + @CustomerName + '%'
			)
		AND (
			@MobileNumber IS NULL
			OR CD.Mobile LIKE '%' + @MobileNumber + '%'
			)
		AND (
			@CarRegistrationNumber IS NULL
			OR TSR.RegistrationNumberSearch LIKE '%' + @CarRegistrationNumber + '%'
			)
	ORDER BY  TSR.ServiceDueDate ASC,CD.CustomerName
			
			
END
