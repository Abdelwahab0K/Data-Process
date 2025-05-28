using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace LandingPage.Models
{
    [Table("Suivi_Process_Stk")]
    public class ProcessDatabase
    {
        [Key]
        [Column("pce_id")]
        [JsonPropertyName("pceId")]
        public string PceId { get; set; } = string.Empty;

        [Column("Journée")]
        [JsonPropertyName("journee")]
        public string? Journee { get; set; }

        [Column("Nmic")]
        [JsonPropertyName("nmic")]
        public string? Nmic { get; set; }

        [Column("coil_id")]
        [JsonPropertyName("coilId")]
        public string? CoilId { get; set; }

        [Column("start_time")]
        [JsonPropertyName("startTime")]
        public DateTime? StartTime { get; set; }

        [Column("end_time")]
        [JsonPropertyName("endTime")]
        public DateTime? EndTime { get; set; }

        [Column("Nombre_pass")]
        [JsonPropertyName("nombrePass")]
        public int? NombrePass { get; set; }

        [Column("Restart")]
        [JsonPropertyName("restart")]
        public string? Restart { get; set; }

        [Column("Cobble")]
        [JsonPropertyName("cobble")]
        public int? Cobble { get; set; }

        [Column("Nature")]
        [JsonPropertyName("nature")]
        public string? Nature { get; set; }

        [Column("Grade")]
        [JsonPropertyName("grade")]
        public string? Grade { get; set; }

        [Column("Fournisseur_brame")]
        [JsonPropertyName("fournisseur_brame")]
        public string? Fournisseur_brame { get; set; }

        [Column("Client")]
        [JsonPropertyName("client")]
        public string? Client { get; set; }

        [Column("Poids_slab_balance_Four")]
        [JsonPropertyName("poids_slab_balance_Four")]
        public float? Poids_slab_balance_Four { get; set; }

        [Column("Poids_Bobine")]
        [JsonPropertyName("poids_Bobine")]
        public float? Poids_Bobine { get; set; }

        [Column("tx_chutes_brute")]
        [JsonPropertyName("tx_chutes_brute")]
        public float? tx_chutes_brute { get; set; }

        [Column("tx_chutes_fin")]
        [JsonPropertyName("tx_chutes_fin")]
        public float? tx_chutes_fin { get; set; }

        [Column("tx_chutes_echantillon")]
        [JsonPropertyName("tx_chutes_echantillon")]
        public float? tx_chutes_echantillon { get; set; }

        [Column("Forme_brame")]
        [JsonPropertyName("forme_brame")]
        public int? Forme_brame { get; set; }

        [Column("slab_len")]
        [JsonPropertyName("slab_len")]
        public float? slab_len { get; set; }

        [Column("slab_wid_min")]
        [JsonPropertyName("slab_wid_min")]
        public float? slab_wid_min { get; set; }

        [Column("slab_wid_max")]
        [JsonPropertyName("slab_wid_max")]
        public float? slab_wid_max { get; set; }

        [Column("largeur_min")]
        [JsonPropertyName("largeur_min")]
        public float? largeur_min { get; set; }

        [Column("largeur_max")]
        [JsonPropertyName("largeur_max")]
        public float? largeur_max { get; set; }

        [Column("Variation_largeur")]
        [JsonPropertyName("variation_largeur")]
        public float? Variation_largeur { get; set; }

        [Column("NC_largeur")]
        [JsonPropertyName("nc_largeur")]
        public int? NC_largeur { get; set; }

        [Column("larg_moy_filtré")]
        [JsonPropertyName("larg_moy_filtré")]
        public float? larg_moy_filtré { get; set; }

        [Column("larg_min_filtré")]
        [JsonPropertyName("larg_min_filtré")]
        public float? larg_min_filtré { get; set; }

        [Column("larg_max_filtré")]
        [JsonPropertyName("larg_max_filtré")]
        public float? larg_max_filtré { get; set; }

        [Column("Defaut_sous_Largeur")]
        [JsonPropertyName("defaut_sous_largeur")]
        public int? Defaut_sous_Largeur { get; set; }

        [Column("Defaut_sur_Largeur")]
        [JsonPropertyName("defaut_sur_largeur")]
        public int? Defaut_sur_Largeur { get; set; }

        [Column("edger_use")]
        [JsonPropertyName("edger_use")]
        public int? edger_use { get; set; }

        [Column("Nb_pass_edger")]
        [JsonPropertyName("nb_pass_edger")]
        public int? Nb_pass_edger { get; set; }

        [Column("Width_bias_obj")]
        [JsonPropertyName("width_bias_obj")]
        public float? Width_bias_obj { get; set; }

        [Column("Width_bias")]
        [JsonPropertyName("width_bias")]
        public float? Width_bias { get; set; }

        [Column("looper_bias")]
        [JsonPropertyName("looper_bias")]
        public float? looper_bias { get; set; }
    }
}
