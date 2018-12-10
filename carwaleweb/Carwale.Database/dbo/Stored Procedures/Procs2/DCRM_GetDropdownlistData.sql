IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetDropdownlistData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetDropdownlistData]
GO

	

-- =============================================
-- Author	:	Ajay Singh(7th jan 2016)
-- Description	:to get oprusers and business unit and designation
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_GetDropdownlistData]

AS
         BEGIN
		 --GET ALL OPRUSERS
               SELECT DISTINCT(OU.UserName)  AS Text,OU.Id AS Value
			   FROM OprUsers AS OU WITH(NOLOCK)
			   WHERE OU.IsActive=1	
			   ORDER BY OU.UserName

          --GET ALL BUSINESSUNIT
			   SELECT HR_DepartmentId AS Value ,DepartmentName AS Text
			   FROM HR_Department AS HDM WITH(NOLOCK)
			   ORDER BY DepartmentName


          --GET ALL DESIGNATION
			   SELECT Id AS Value,Designation AS Text
			   FROM HR_Designation AS HD WITH(NOLOCK)
			   ORDER BY Designation
	
         END
