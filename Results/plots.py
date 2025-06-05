import matplotlib.pyplot as plt
import numpy as np

plt.style.use('seaborn-v0_8-darkgrid')

words = []
real = []
expected = []

with open('graph_bi_data.txt', 'r', encoding='utf-8') as f:
    for line in f:
        parts = line.replace(',', '.').strip().split(' ')
        words.append(parts[0])
        real.append(float(parts[1]))
        expected.append(float(parts[2]))

words = words[:100]
real = real[:100]
expected = expected[:100]

n = len(words)
width = 0.4
x = np.arange(n)
figsize = (25, 15) 

fig, ax = plt.subplots(figsize=figsize)

rects1 = ax.bar(x - width/2, real, width, 
               label='Реальная частота', 
               color='#2e7d32',
               edgecolor='black',
               linewidth=0.5,
               alpha=0.9)

rects2 = ax.bar(x + width/2, expected, width, 
               label='Ожидаемая частота', 
               color='#c62828',
               edgecolor='black',
               linewidth=0.5,
               alpha=0.9)

ax.set_title('Сравнение частот биграмм (топ-100)', 
             fontsize=20, pad=25, fontweight='bold', fontfamily='Arial')
ax.set_ylabel('Частота', fontsize=16, fontfamily='Arial')
ax.set_xlabel('Биграммы', fontsize=16, fontfamily='Arial')

ax.set_xticks(x)
ax.set_xticklabels(
    words,
    rotation=85,
    ha='center',
    va='top',
    fontsize=10,
    fontfamily='Arial'
)

for xtick in x:
    ax.axvline(xtick, color='gray', alpha=0.2, linestyle=':')

legend = ax.legend(fontsize=14, loc='upper right',
                  framealpha=1, edgecolor='black')
legend.get_frame().set_facecolor('#f5f5f5')

ax.grid(axis='y', alpha=0.4, linestyle='--')
ax.grid(axis='x', alpha=0.1)
ax.set_xlim(-0.5, n-0.5)

plt.text(0.01, 0.99, 'Vlasov S.', 
         transform=ax.transAxes, 
         fontsize=12, 
         verticalalignment='top',
         horizontalalignment='left',
         bbox=dict(facecolor='white', alpha=0.8))

plt.tight_layout()
plt.savefig('gen-1.png', dpi=120, bbox_inches='tight')

words = []
real = []
expected = []

with open('graph_word_data.txt', 'r', encoding='utf-8') as f:
    for line in f:
        parts = line.replace(',', '.').strip().split(' ')
        words.append(parts[0])
        real.append(float(parts[1]))
        expected.append(float(parts[2]))

words = words[:100]
real = real[:100]
expected = expected[:100]

n = len(words)
width = 0.4
x = np.arange(n)
figsize = (25, 15) 

fig, ax = plt.subplots(figsize=figsize)

rects1 = ax.bar(x - width/2, real, width, 
               label='Реальная частота', 
               color='#1565c0',
               hatch='//',
               edgecolor='white',
               alpha=0.8)

rects2 = ax.bar(x + width/2, expected, width, 
               label='Ожидаемая частота', 
               color='#6a1b9a',
               hatch='\\\\',
               edgecolor='white',
               alpha=0.8)

ax.set_title('Сравнение частот слов (топ-100)', 
             fontsize=20, pad=25, fontweight='bold', fontfamily='Arial',
             color='#333333')
ax.set_ylabel('Частота', fontsize=16, fontfamily='Arial')
ax.set_xlabel('Слова', fontsize=16, fontfamily='Arial')

ax.set_xticks(x)
ax.set_xticklabels(
    words,
    rotation=85,
    ha='center',
    va='top',
    fontsize=10,
    fontfamily='Arial',
    fontweight='light'
)

for xtick in x:
    ax.axvline(xtick, color='gray', alpha=0.15, linestyle='-')

legend = ax.legend(fontsize=14, loc='upper right',
                  framealpha=1, edgecolor='black')
legend.get_frame().set_linewidth(1.5)

ax.grid(axis='y', alpha=0.5)
ax.set_xlim(-0.5, n-0.5)

mean_real = np.mean(real)
mean_expected = np.mean(expected)
ax.axhline(mean_real, color='#1565c0', linestyle='--', alpha=0.7, linewidth=1.5)
ax.axhline(mean_expected, color='#6a1b9a', linestyle='--', alpha=0.7, linewidth=1.5)


ax.text(n, mean_real, f' Среднее: {mean_real:.2e}', 
        va='center', ha='left', color='#1565c0', fontsize=12)
ax.text(n, mean_expected, f' Среднее: {mean_expected:.2e}', 
        va='center', ha='left', color='#6a1b9a', fontsize=12)

plt.text(0.01, 0.99, 'Vlasov S.', 
         transform=ax.transAxes, 
         fontsize=12, 
         verticalalignment='top',
         horizontalalignment='left',
         bbox=dict(facecolor='white', alpha=0.8))

plt.tight_layout()
plt.savefig('gen-2.png', dpi=120, bbox_inches='tight')