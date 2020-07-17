<?xml version="1.0" encoding="UTF-8"?>

<xsl:stylesheet version="1.0"
   xmlns:xsl="http://www.w3.org/1999/XSL/Transform">

  <xsl:template match="/">
    <html>
      <head>
          <title>schedule</title>
        </head>
      <body bgcolor="#F1AEA2" text-align="center" font-family="Calibry">
        <table bgcolor="#AA5E51" border="1"  margin="o auto">
          <tr bgcolor="#D47564">
            <th>day</th>
            <th>lesson</th>
          </tr>
          <xsl:for-each select="schedule/week/day">
            <tr>
              <th bgcolor="#D47564">
                <xsl:value-of select="@name_day"/>
              </th>
              <td>
                <table border="1">
                  <xsl:for-each select="lesson">
                    <tr>
                      <td>
                        <xsl:value-of select="@title"/>
                      </td>
                      <td>
                        <xsl:value-of select="class"/>
                      </td>
                      <td>
                        <xsl:value-of select="teacher"/>
                      </td>
                      <td>
                        <xsl:value-of select="time/start"/>---<xsl:value-of select="time/end"/>
                      </td>
                      <td>
                        <xsl:value-of select="type"/>
                      </td>
                    </tr>
                  </xsl:for-each>
                </table>
              </td>
            </tr>
          </xsl:for-each>
        </table>
      </body>
    </html>
  </xsl:template>

</xsl:stylesheet>