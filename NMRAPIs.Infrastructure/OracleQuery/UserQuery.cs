// <copyright file="UserQuery.cs" company="NMRAPIs">
// This source code is owned by NMRAPIs and is not allowed to be copied, reproduced,
// published, distributed or transmitted to or stored in any manner without prior
// written consent from NMRAPIs. www.NMRAPIs.com.
// </copyright>

namespace NMRAPIs.Infrastructure.OracleQuery
{
    /// <summary>
    /// User Query Class.
    /// </summary>
    public static class UserQuery
    {
        /// <summary>
        /// Query to get all users.
        /// </summary>
        public const string GetAllUsers = @"SELECT
            cm.contactiid,
            trim(c.firstname) || ' ' || trim(c.lastname) as name,
            c.emailaddr as email
            FROM
            crewmaster cm
            INNER JOIN contact c
            ON c.contactiid = cm.contactiid
            WHERE cm.active = 1 AND cm.crewtype = 0
              --0 = inhouse 1 = freelancer
            ORDER BY name";

        /// <summary>
        /// Query to get all Addresses.
        /// </summary>
        public const string GetAllAddresses = @"SELECT DISTINCT * FROM
            (SELECT DISTINCT
            addressiid,
            addressline || ', ' || city || ', ' || state || ' ' || zip as FullAddress
            FROM address ) a
            WHERE UPPER(a.FullAddress) LIKE UPPER('%'|| {0} || '%')
            ORDER BY FullAddress";

        /// <summary>
        /// Query to get Address by id.
        /// </summary>
        public const string GetAddressById = @"SELECT
        addressiid,
        addressline || ', ' || city || ', ' || state || ' ' || zip as FullAddress
        FROM address a
        WHERE addressiid = {0}";

        /// <summary>
        /// Query to get all departments.
        /// </summary>
        public const string GetAllDepartments = @"Select
        Groupiid,
        Groupid,
        GetInvGroupsDescription(GroupIID) Description 
        from InvGroups
        where grouptype=13";

        /// <summary>
        /// Query to get Department by id.
        /// </summary>
        public const string GetDepartmentById = @"Select
        Groupiid,
        Groupid,
        GetInvGroupsDescription(GroupIID) Description 
        from InvGroups
        where grouptype=13 AND Groupiid = {0}";

        /// <summary>
        /// Query to get all Addresses.
        /// </summary>
        public const string GetEventById = @"SELECT 
          cv.contractid, 
          cv.type, 
          cv.contractdescription, 
          cv.eventid, 
          cv.datecreated, 
          cv.loadindate, 
          cv.pickupdate, 
          cv.salesperson, 
          cv.sitename, 
          cv.shippingcity || ', ' || cv.shippingstate As City, 
          l.locdescription, 
          c.lastupdated, 
          cv.shippingboothroomno, 
          (
            SELECT 
              SUM(amount) as TravelExpenses 
            FROM 
              CONTRACTLINE cl2 
              INNER JOIN invmaster i2 ON i2.inviid = cl2.inviid 
              INNER JOIN contract c2 on c2.contractiid = cl2.contractiid 
            WHERE 
              cl2.contractiid = c.contractiid 
              AND itemtaxgroup = 'TRAVEL'
          ) TravelExpenses, 
          (
            SELECT 
              SUM(cl2.amount) 
            FROM 
              CONTRACTLABORLINE cl2 
              INNER JOIN invmaster i2 ON i2.inviid = cl2.inviid 
            WHERE 
              sku NOT like '%UNION%' 
              AND cl2.contractiid = c.contractiid
          ) NonUnionLabor, 
          (
            SELECT 
              SUM(cl2.amount) 
            FROM 
              CONTRACTLABORLINE cl2 
              INNER JOIN invmaster i2 ON i2.inviid = cl2.inviid 
            WHERE 
              sku like '%UNION%' 
              AND cl2.contractiid = c.contractiid
          ) UnionLabor, 
          cv.laboramount as LaborTotal, 
          cv.freightamount as FreightTotal, 
          cv.orderamount as OrderTotal, 
          nvl(
            (
              SELECT 
                labornotifyiconcolor as iconcolor 
              FROM 
                allorderview a2 
              WHERE 
                a2.contractid = cv.contractid
            ), 
            ''
          ) as NotifyColor, 
          nvl(
            (
              SELECT 
                totalnooffilled 
              FROM 
                allorderview a2 
              WHERE 
                a2.contractid = cv.contractid
            ), 
            0
          ) as LaborFilled, 
          nvl(
            (
              SELECT 
                totalnoofconfirmed 
              FROM 
                allorderview a2 
              WHERE 
                a2.contractid = cv.contractid
            ), 
            0
          ) as LaborConfirmed, 
          (
            SELECT 
              CASE nvl(
                SUM(QUANTITY), 
                0
              ) WHEN 0 THEN 0 ELSE ROUND(
                (
                  SUM(Filled) / SUM(QUANTITY)
                ), 
                2
              ) * 100 END AS Percentage 
            FROM 
              (
                SELECT 
                  SUM(
                    MAX(CONTRACTLINE.QUANTITY)
                  ) AS QUANTITY, 
                  SUM(
                    CASE WHEN COUNT(
                      CONTRACTLINESERIAL.SERIALNUMBER
                    ) = 0 THEN CASE MAX(contractline.Status) WHEN 9 THEN MAX(CONTRACTLINE.QUANTITY) WHEN 13 THEN MAX(CONTRACTLINE.QUANTITY) WHEN 16 THEN MAX(CONTRACTLINE.QUANTITY) WHEN 24 THEN MAX(CONTRACTLINE.QUANTITY) ELSE COUNT(
                      CONTRACTLINESERIAL.SERIALNUMBER
                    ) END ELSE COUNT(
                      CONTRACTLINESERIAL.SERIALNUMBER
                    ) END
                  ) AS FILLED 
                FROM 
                  CONTRACTLINE 
                  INNER JOIN CONTRACTLINESERIAL ON CONTRACTLINESERIAL.CONTRACTIID = CONTRACTLINE.CONTRACTIID 
                  AND CONTRACTLINESERIAL.LINENO = CONTRACTLINE.LINENO 
                  LEFT OUTER JOIN CONTRACT ON CONTRACTLINE.CONTRACTIID = CONTRACT.CONTRACTIID 
                WHERE 
                  CONTRACT.contractid = cv.contractid 
                  AND contractline.Status IN (3, 6, 13, 14, 16, 24) 
                GROUP BY 
                  CONTRACTLINE.LINENO, 
                  CONTRACTLINE.LEVELNO
              ) Table1
          ) As FilledPercentage 
        FROM 
          REP_ORDERHEADERVIEW cv 
          INNER JOIN contract c ON c.contractid = cv.contractid 
          LEFT OUTER JOIN location l ON l.locname = cv.shippinglocation 
        WHERE 
          cv.eventid = UPPER({0}) 
          AND (
            c.type <> 0 
            AND c.type <> 2 
            AND c.status <> 3
          ) 
        ORDER BY 
          cv.loadindate, 
          cv.contractid";
    }
}
